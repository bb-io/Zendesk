using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using Apps.Zendesk.Models.DynamicContent;
using Apps.Zendesk.Models.Requests;
using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Filters.Constants;
using Blackbird.Filters.Extensions;
using Blackbird.Filters.Transformations;
using RestSharp;

namespace Apps.Zendesk.Actions;

[ActionList("Dynamic content")]
public class DynamicContentActions(
    InvocationContext invocationContext,
    IFileManagementClient fileManagementClient) : BaseInvocable(invocationContext)
{
    private readonly ZendeskClient _client = new(invocationContext);
    private readonly IFileManagementClient _fileManagementClient = fileManagementClient;

    [Action("Search dynamic content", Description = "Search dynamic content items by name or placeholder")]
    public async Task<SearchDynamicContentResponse> SearchDynamicContent(
        [ActionParameter] SearchDynamicContentRequest input)
    {
        var items = await GetAllItems();
        if (!string.IsNullOrWhiteSpace(input.Query))
        {
            var query = input.Query.Trim();
            items = items
                .Where(x => x.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                    || x.Placeholder.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return new SearchDynamicContentResponse
        {
            Items = items,
            TotalCount = items.Count
        };
    }

    [Action("Find dynamic content", Description = "Find a dynamic content item by ID or placeholder")]
    public async Task<FindDynamicContentResponse> FindDynamicContent(
        [ActionParameter] FindDynamicContentRequest input)
    {
        var hasId = !string.IsNullOrWhiteSpace(input.DynamicContentId);
        var hasPlaceholder = !string.IsNullOrWhiteSpace(input.Placeholder);
        if (hasId == hasPlaceholder)
        {
            throw new PluginMisconfigurationException(
                "Provide exactly one of Dynamic content ID or Placeholder.");
        }

        if (hasId)
        {
            ValidateId(input.DynamicContentId!);
            try
            {
                return new FindDynamicContentResponse
                {
                    Found = true,
                    Item = await GetItem(input.DynamicContentId!)
                };
            }
            catch (PluginApplicationException exception)
                when (IsNotFound(exception))
            {
                return new FindDynamicContentResponse { Found = false };
            }
        }

        var normalizedPlaceholder = NormalizePlaceholder(input.Placeholder!);
        var item = (await GetAllItems()).FirstOrDefault(x =>
            string.Equals(NormalizePlaceholder(x.Placeholder), normalizedPlaceholder,
                StringComparison.OrdinalIgnoreCase));

        return new FindDynamicContentResponse
        {
            Found = item is not null,
            Item = item
        };
    }

    [Action("Download dynamic content", Description = "Download one dynamic content variant as a translatable HTML file")]
    public async Task<DownloadDynamicContentResponse> DownloadDynamicContent(
        [ActionParameter] DownloadDynamicContentRequest input)
    {
        ValidateId(input.DynamicContentId);
        var contentFormat = NormalizeContentFormat(input.ContentFormat);
        var item = await GetItem(input.DynamicContentId);
        var locales = await GetSupportLocales();

        SupportLocale? selectedLocale;
        DynamicContentVariant? selectedVariant;
        if (string.IsNullOrWhiteSpace(input.SourceLanguage))
        {
            selectedLocale = locales.FirstOrDefault(x => x.Id == item.DefaultLocaleId);
            selectedVariant = item.Variants.FirstOrDefault(x => x.LocaleId == item.DefaultLocaleId)
                ?? item.Variants.FirstOrDefault(x => x.Default);
        }
        else
        {
            selectedLocale = locales.FirstOrDefault(x => string.Equals(
                x.LocaleCode, input.SourceLanguage.Trim(), StringComparison.OrdinalIgnoreCase));
            if (selectedLocale is null)
            {
                var available = string.Join(", ", locales.Select(x => x.LocaleCode));
                throw new PluginMisconfigurationException(
                    $"Source language '{input.SourceLanguage}' is not enabled in Zendesk Support. Available languages: {available}.");
            }

            selectedVariant = item.Variants.FirstOrDefault(x => x.LocaleId == selectedLocale.Id);
        }

        if (selectedVariant is null)
        {
            throw new PluginMisconfigurationException(
                $"Dynamic content item '{item.Id}' does not have a variant for the selected source language.");
        }

        selectedLocale ??= locales.FirstOrDefault(x => x.Id == selectedVariant.LocaleId);
        if (selectedLocale is null)
        {
            throw new PluginApplicationException(
                $"Zendesk locale ID '{selectedVariant.LocaleId}' was not returned by the Support Locales API.");
        }

        var adminUrl = string.IsNullOrWhiteSpace(item.Url)
            ? $"{_client.BaseUrl}/api/v2/dynamic_content/items/{item.Id}"
            : item.Url;
        var html = DynamicContentHtmlConverter.Convert(
            item,
            selectedVariant,
            selectedLocale.LocaleCode,
            contentFormat,
            _client.BaseUrl,
            adminUrl);

        var fileName = $"{SanitizeFileName(item.Name, item.Id)}_{selectedLocale.LocaleCode}.html";
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
        var file = await _fileManagementClient.UploadAsync(stream, MediaTypeNames.Text.Html, fileName);

        return new DownloadDynamicContentResponse
        {
            Content = file,
            Item = item,
            Variant = selectedVariant,
            SourceLanguage = selectedLocale.LocaleCode,
            ContentFormat = contentFormat
        };
    }

    [Action("Upload dynamic content", Description = "Create or update a dynamic content language variant from a translated HTML or XLIFF file")]
    public async Task<UploadDynamicContentResponse> UploadDynamicContent(
        [ActionParameter] UploadDynamicContentRequest input)
    {
        ValidateUploadExtension(input.Content.Name);

        using var fileStream = await _fileManagementClient.DownloadAsync(input.Content);
        var transformationResult = Transformation.Load(
            fileStream,
            input.Content.Name,
            input.Content.ContentType);
        if (!transformationResult.Success)
        {
            throw new PluginMisconfigurationException(
                $"The content file could not be read as supported HTML or XLIFF: {transformationResult.Error}");
        }

        var transformation = transformationResult.Value;
        var targetResult = transformation.Target();
        if (!targetResult.Success)
        {
            throw new PluginMisconfigurationException(
                $"The translated content could not be extracted from the file: {targetResult.Error}");
        }

        var target = targetResult.Value;
        DynamicContentHtmlConverter.ParsedContent parsed;
        try
        {
            parsed = DynamicContentHtmlConverter.Parse(target.ToStream().ReadString());
        }
        catch (InvalidDataException exception)
        {
            throw new PluginMisconfigurationException(exception.Message);
        }
        var itemId = input.DynamicContentId?.Trim()
            ?? parsed.DynamicContentId
            ?? transformation.SourceSystemReference.ContentId;
        if (string.IsNullOrWhiteSpace(itemId))
        {
            throw new PluginMisconfigurationException(
                "Dynamic content ID is missing. Provide it in the action input or use a file produced by Download dynamic content.");
        }

        ValidateId(itemId);

        var targetLanguage = input.TargetLanguage?.Trim()
            ?? transformation.TargetLanguage
            ?? target.Language
            ?? transformation.SourceLanguage;
        if (string.IsNullOrWhiteSpace(targetLanguage))
        {
            throw new PluginMisconfigurationException(
                "Target language is missing. Provide it in the action input or in the translated file metadata.");
        }

        var locales = await GetSupportLocales();
        var targetLocale = locales.FirstOrDefault(x => string.Equals(
            x.LocaleCode, targetLanguage, StringComparison.OrdinalIgnoreCase));
        if (targetLocale is null)
        {
            var available = string.Join(", ", locales.Select(x => x.LocaleCode));
            throw new PluginMisconfigurationException(
                $"Target language '{targetLanguage}' is not enabled in Zendesk Support. Available languages: {available}.");
        }

        var item = await GetItem(itemId);
        var variant = item.Variants.FirstOrDefault(x => x.LocaleId == targetLocale.Id);
        string operation;

        if (variant is null)
        {
            var createRequest = new ZendeskRequest(
                $"/api/v2/dynamic_content/items/{item.Id}/variants", Method.Post);
            createRequest.AddNewtonJson(new
            {
                variant = new
                {
                    locale_id = targetLocale.Id,
                    active = input.Active ?? true,
                    content = parsed.Content
                }
            });
            variant = (await _client.ExecuteWithHandling<DynamicContentVariantResponse>(createRequest)).Variant;
            operation = "created";
        }
        else
        {
            var active = input.Active ?? variant.Active;
            if (variant.Content == parsed.Content && variant.Active == active)
            {
                operation = "unchanged";
            }
            else
            {
                var updateRequest = new ZendeskRequest(
                    $"/api/v2/dynamic_content/items/{item.Id}/variants/{variant.Id}", Method.Put);
                updateRequest.AddNewtonJson(new
                {
                    variant = new
                    {
                        active,
                        content = parsed.Content
                    }
                });
                variant = (await _client.ExecuteWithHandling<DynamicContentVariantResponse>(updateRequest)).Variant;
                operation = "updated";
            }
        }

        var adminUrl = string.IsNullOrWhiteSpace(item.Url)
            ? $"{_client.BaseUrl}/api/v2/dynamic_content/items/{item.Id}"
            : item.Url;
        transformation.TargetSystemReference.ContentId = item.Id;
        transformation.TargetSystemReference.ContentName = item.Name;
        transformation.TargetSystemReference.AdminUrl = adminUrl;
        transformation.TargetSystemReference.SystemName = "Zendesk";
        transformation.TargetSystemReference.SystemRef = _client.BaseUrl;
        transformation.TargetLanguage = targetLocale.LocaleCode;

        FileReference outputFile;
        if (transformationResult.WasBilingual)
        {
            outputFile = await _fileManagementClient.UploadAsync(
                transformation.ToStream(),
                MediaTypes.Xliff2,
                transformation.BilingualFileName);
        }
        else
        {
            target.SystemReference = transformation.TargetSystemReference;
            target.Language = targetLocale.LocaleCode;
            outputFile = await _fileManagementClient.UploadAsync(
                target.ToStream(),
                target.OriginalMediaType,
                target.OriginalName);
        }

        return new UploadDynamicContentResponse
        {
            Content = outputFile,
            DynamicContentId = item.Id,
            VariantId = variant.Id,
            TargetLanguage = targetLocale.LocaleCode,
            Operation = operation
        };
    }

    public static string NormalizePlaceholder(string placeholder)
    {
        var value = placeholder.Trim();
        if (value.StartsWith("{{", StringComparison.Ordinal) &&
            value.EndsWith("}}", StringComparison.Ordinal) && value.Length >= 4)
        {
            value = value[2..^2].Trim();
        }

        if (value.StartsWith("dc.", StringComparison.OrdinalIgnoreCase))
            value = value[3..];

        if (string.IsNullOrWhiteSpace(value))
            throw new PluginMisconfigurationException("Placeholder cannot be empty.");

        return value;
    }

    private async Task<List<DynamicContentItem>> GetAllItems()
    {
        var items = new List<DynamicContentItem>();
        string? nextPage = "/api/v2/dynamic_content/items?page[size]=100";
        var visitedPages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        while (!string.IsNullOrWhiteSpace(nextPage))
        {
            if (!visitedPages.Add(nextPage))
                throw new PluginApplicationException("Zendesk returned a repeated pagination URL for dynamic content items.");

            var response = await _client.ExecuteWithHandling<DynamicContentItemsResponse>(
                new ZendeskRequest(nextPage, Method.Get));
            items.AddRange(response.Items);
            nextPage = response.NextPage;
        }

        return items;
    }

    private async Task<DynamicContentItem> GetItem(string id)
    {
        var response = await _client.ExecuteWithHandling<DynamicContentItemResponse>(
            new ZendeskRequest($"/api/v2/dynamic_content/items/{id}", Method.Get));
        return response.Item;
    }

    private async Task<List<SupportLocale>> GetSupportLocales()
    {
        var response = await _client.ExecuteWithHandling<SupportLocalesResponse>(
            new ZendeskRequest("/api/v2/locales", Method.Get));
        return response.Locales;
    }

    private static string NormalizeContentFormat(string? contentFormat)
    {
        if (string.IsNullOrWhiteSpace(contentFormat))
            return DynamicContentHtmlConverter.TextFormat;

        if (contentFormat.Equals(DynamicContentHtmlConverter.TextFormat, StringComparison.OrdinalIgnoreCase))
            return DynamicContentHtmlConverter.TextFormat;
        if (contentFormat.Equals(DynamicContentHtmlConverter.HtmlFormat, StringComparison.OrdinalIgnoreCase))
            return DynamicContentHtmlConverter.HtmlFormat;

        throw new PluginMisconfigurationException("Content format must be Text or HTML.");
    }

    private static void ValidateId(string id)
    {
        if (!long.TryParse(id, out var parsed) || parsed <= 0)
            throw new PluginMisconfigurationException("Dynamic content ID must be a positive integer.");
    }

    private static void ValidateUploadExtension(string fileName)
    {
        var extension = Path.GetExtension(fileName);
        if (!extension.Equals(".html", StringComparison.OrdinalIgnoreCase)
            && !extension.Equals(".xliff", StringComparison.OrdinalIgnoreCase)
            && !extension.Equals(".xlf", StringComparison.OrdinalIgnoreCase))
        {
            throw new PluginMisconfigurationException(
                "Only .html, .xliff, and .xlf files are supported.");
        }
    }

    private static bool IsNotFound(PluginApplicationException exception) =>
        exception.Message.Equals("Error: RecordNotFound", StringComparison.OrdinalIgnoreCase)
        || exception.Message.Equals("Error: NotFound", StringComparison.OrdinalIgnoreCase);

    private static string SanitizeFileName(string name, string fallback)
    {
        var invalidChars = Regex.Escape(new string(Path.GetInvalidFileNameChars()));
        var sanitized = Regex.Replace(name, $"[{invalidChars}]", string.Empty).Trim();
        return string.IsNullOrWhiteSpace(sanitized) ? fallback : sanitized;
    }
}
