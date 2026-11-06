using Apps.Zendesk.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Filters.Constants;
using Blackbird.Filters.Coders;
using Blackbird.Filters.Enums;
using Blackbird.Filters.Shared;
using Blackbird.Filters.Extensions;
using Blackbird.Filters.Transformations;
using HtmlAgilityPack;
using System.Net;
using System.Text;

namespace Apps.Zendesk.Actions;

[ActionList("Dynamic content")]
public class DynamicContentActions(InvocationContext context, IFileManagementClient files) : BaseInvocable(context)
{
    private ZendeskClient Client { get; } = new(context);
    private DynamicContentApi Api => new(Client);

    [Action("Search dynamic content", Description = "Search dynamic content by name or placeholder")]
    public async Task<DynamicSearchOutput> Search([ActionParameter] DynamicSearchInput input)
    {
        var items = (await Api.List()).Where(x => string.IsNullOrWhiteSpace(input.Query) || x.Name.Contains(input.Query, StringComparison.OrdinalIgnoreCase) || x.Placeholder.Contains(input.Query, StringComparison.OrdinalIgnoreCase)).ToList();
        return new() { Items = items, TotalCount = items.Count };
    }
    [Action("Find dynamic content", Description = "Find dynamic content by ID or exact placeholder")]
    public async Task<DynamicFindOutput> Find([ActionParameter] DynamicFindInput input)
    {
        if (string.IsNullOrWhiteSpace(input.Id) == string.IsNullOrWhiteSpace(input.Placeholder)) throw new PluginMisconfigurationException("Provide exactly one dynamic content ID or placeholder.");
        var item = !string.IsNullOrWhiteSpace(input.Id) ? await Api.Get(input.Id) : (await Api.List()).FirstOrDefault(x => string.Equals(DynamicContentApi.NormalizePlaceholder(x.Placeholder), DynamicContentApi.NormalizePlaceholder(input.Placeholder!), StringComparison.OrdinalIgnoreCase));
        return new() { Found = item != null, Item = item };
    }
    [Action("Download dynamic content", Description = "Download a dynamic content variant for translation")]
    public async Task<DynamicDownloadOutput> Download([ActionParameter] DynamicIdInput input, [ActionParameter] DynamicDownloadInput options)
    {
        var item = await Api.Get(input.Id) ?? throw new PluginMisconfigurationException("Dynamic content was not found.");
        var variant = string.IsNullOrWhiteSpace(options.Language) ? item.Variants.FirstOrDefault(x => x.Default || x.LocaleId == item.DefaultLocaleId) : item.Variants.FirstOrDefault(x => string.Equals(x.Language, options.Language, StringComparison.OrdinalIgnoreCase));
        if (variant == null || string.IsNullOrWhiteSpace(variant.Language)) throw new PluginMisconfigurationException("The requested source language variant was not found or is not enabled in Support.");
        var format = options.Format ?? "Text";
        if (format != "Text" && format != "HTML") throw new PluginMisconfigurationException("Content format must be Text or HTML.");
        var html = DynamicContentFile.Create(item, variant, format, Client.BaseUrl);
        var filename = string.Concat(item.Name.Select(c => Path.GetInvalidFileNameChars().Contains(c) || "<>:\"/\\|?*".Contains(c) ? '_' : c)).Trim();
        if (string.IsNullOrEmpty(filename)) filename = "dynamic-content";
        var file = await files.UploadAsync(new MemoryStream(Encoding.UTF8.GetBytes(html)), "text/html", $"{filename}_{variant.Language}.html");
        return new() { Content = file, Item = item, Variant = variant };
    }
    [Action("Upload dynamic content", Description = "Create or update a dynamic content language variant from a translated file")]
    public async Task<DynamicUploadOutput> Upload([ActionParameter] DynamicUploadInput input)
    {
        using var stream = await files.DownloadAsync(input.Content);
        var loaded = Transformation.Load(stream, input.Content.Name, input.Content.ContentType);
        var transformation = loaded.Value ?? throw new PluginMisconfigurationException(loaded.Error ?? "Unable to load the content file.");
        var targetResult = loaded.Target();
        if (!targetResult.Success || targetResult.Value == null) throw new PluginMisconfigurationException(targetResult.Error ?? "The file does not contain target content.");
        var target = targetResult.Value;
        using var targetStream = target.ToStream();
        using var reader = new StreamReader(targetStream);
        var html = await reader.ReadToEndAsync();
        var (embeddedId, content) = DynamicContentFile.Extract(html);
        var id = string.IsNullOrWhiteSpace(input.Id) ? embeddedId : input.Id;
        if (string.IsNullOrWhiteSpace(id)) throw new PluginMisconfigurationException("Dynamic content ID is missing from the file and input.");
        var language = string.IsNullOrWhiteSpace(input.Language) ? transformation.TargetLanguage : input.Language;
        if (string.IsNullOrWhiteSpace(language)) throw new PluginMisconfigurationException("Provide a target language or use a file with target-language metadata.");
        var locale = (await Api.Locales()).FirstOrDefault(x => string.Equals(x.Code, language, StringComparison.OrdinalIgnoreCase)) ?? throw new PluginMisconfigurationException($"Target language '{language}' is not enabled in Zendesk Support.");
        var item = await Api.Get(id) ?? throw new PluginMisconfigurationException("Dynamic content was not found.");
        var existing = item.Variants.FirstOrDefault(x => x.LocaleId == locale.Id);
        var unchanged = existing != null && existing.Content == content && (input.Active == null || input.Active == existing.Active);
        var variant = unchanged ? existing! : await Api.Save(id, existing, locale, content, input.Active);
        var operation = unchanged ? "unchanged" : existing == null ? "created" : "updated";
        var reference = transformation.TargetSystemReference;
        reference.ContentId = $"{new Uri(Client.BaseUrl).Host}:dynamic-content:{id}";
        reference.ContentName = item.Name;
        reference.AdminUrl = $"{Client.BaseUrl}/api/v2/dynamic_content/items/{id}";
        reference.SystemName = "Zendesk";
        reference.SystemRef = Client.BaseUrl;
        transformation.TargetLanguage = locale.Code;
        Stream output;
        string mime;
        string name;
        if (loaded.WasBilingual) { output = transformation.ToStream(); mime = MediaTypes.Xliff2; name = transformation.BilingualFileName; }
        else { target.SystemReference = reference; target.Language = locale.Code; output = target.ToStream(); mime = "text/html"; name = input.Content.Name; }
        var file = await files.UploadAsync(output, mime, name);
        return new() { Content = file, Id = id, VariantId = variant.Id, Language = locale.Code, Operation = operation };
    }
}

public static class DynamicContentFile
{
    public static string Create(DynamicItem item, DynamicVariant variant, string format, string baseUrl)
    {
        var host = new Uri(baseUrl).Host;
        string E(string? s) => WebUtility.HtmlEncode(s ?? "");
        string Meta(string name, string value) => $"<meta name=\"{name}\" content=\"{E(value)}\">";
        var content = format == "Text" ? E(variant.Content).Replace("\r\n", "<br data-zendesk-break=\"crlf\">").Replace("\n", "<br data-zendesk-break=\"lf\">").Replace("\r", "<br data-zendesk-break=\"cr\">") : variant.Content;
        var tag = "div";
        var html = $"<html lang=\"{E(variant.Language)}\"><head>" +
            Meta("blackbird-ucid", $"{host}:dynamic-content:{item.Id}") + Meta("blackbird-content-name", item.Name) + Meta("blackbird-system-name", "Zendesk") + Meta("blackbird-system-ref", baseUrl) + Meta("blackbird-admin-url", $"{baseUrl}/api/v2/dynamic_content/items/{item.Id}") +
            $"</head><body its-rev-tool=\"Zendesk\" its-rev-tool-ref=\"{E(baseUrl)}\"><{tag} data-blackbird-key=\"dynamic-content-{E(item.Id)}-content\" data-zendesk-item-id=\"{E(item.Id)}\" data-zendesk-variant-id=\"{E(variant.Id)}\" data-zendesk-placeholder=\"{E(item.Placeholder)}\" data-zendesk-format=\"{E(format)}\">{content}</{tag}></body></html>";
        var coder = new HtmlCoder();
        var coded = coder.Deserialize(html, "dynamic-content.html");
        coded.Language = variant.Language;
        coded.SystemReference = new SystemReference { ContentId = $"{host}:dynamic-content:{item.Id}", ContentName = item.Name, AdminUrl = $"{baseUrl}/api/v2/dynamic_content/items/{item.Id}", SystemName = "Zendesk", SystemRef = baseUrl };
        return coder.Serialize(coded, MetadataHandling.Include);
    }
    public static (string Id, string Content) Extract(string html)
    {
        var doc = new HtmlDocument(); doc.LoadHtml(html);
        var node = doc.DocumentNode.SelectSingleNode("//*[@data-zendesk-item-id and @data-zendesk-format]") ?? throw new PluginMisconfigurationException("The file is missing dynamic content roundtrip metadata.");
        var format = node.GetAttributeValue("data-zendesk-format", "");
        if (format != "Text" && format != "HTML") throw new PluginMisconfigurationException("The file has an invalid dynamic content format.");
        if (format == "Text")
        {
            foreach (var lineBreak in node.SelectNodes(".//br") ?? Enumerable.Empty<HtmlNode>())
                lineBreak.ParentNode.ReplaceChild(doc.CreateTextNode(lineBreak.GetAttributeValue("data-zendesk-break", "lf") switch { "crlf" => "\r\n", "cr" => "\r", _ => "\n" }), lineBreak);
        }
        return (WebUtility.HtmlDecode(node.GetAttributeValue("data-zendesk-item-id", "")), format == "Text" ? WebUtility.HtmlDecode(node.InnerText) : node.InnerHtml);
    }
}
