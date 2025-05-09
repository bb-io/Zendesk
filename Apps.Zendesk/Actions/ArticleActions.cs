using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.Zendesk.Models.Requests;
using RestSharp;
using System.Text;
using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.Zendesk.Models.Identifiers;
using Apps.Zendesk.Models.Responses.Wrappers;
using System.Net.Mime;
using System.Text.RegularExpressions;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Zendesk.Actions;

[ActionList]
public class ArticleActions : BaseInvocable
{
    private readonly IFileManagementClient _fileManagementClient;

    private ZendeskClient Client { get; }

    public ArticleActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
        Client = new ZendeskClient(invocationContext);
    }

    [Action("Search articles", Description = "Search articles using various filter parameters")]
    public async Task<ListArticlesResponse> SearchArticles([ActionParameter] SearchArticlesRequest input)
    {

        if (input.Query == null && input.LabelNames == null && input.CategoryIds == null && input.SectionIds == null)
        {
            throw new PluginMisconfigurationException("At least one of 'Query', 'Category IDs', 'Section Ids' or 'Label names' should be added to the input values.");
        }

        var endpoint = $"/api/v2/help_center/articles/search";
        var request = new ZendeskRequest(endpoint, Method.Get);
        if (input.Query != null) request.AddQueryParameter("query", input.Query);
        if (input.Locale != null) request.AddQueryParameter("locale", input.Locale);

        if (input.CategoryIds != null) request.AddQueryParameter("category", string.Join(',', input.CategoryIds));
        if (input.SectionIds != null) request.AddQueryParameter("section", string.Join(',', input.SectionIds));
        if (input.LabelNames != null) request.AddQueryParameter("label_names", string.Join(',', input.LabelNames));

        if (input.CreatedAfter.HasValue) request.AddQueryParameter("created_after", input.CreatedAfter.Value.ToString("yyyy-MM-dd"));
        if (input.CreatedBefore.HasValue) request.AddQueryParameter("created_before", input.CreatedBefore.Value.ToString("yyyy-MM-dd"));
        if (input.CreatedAt.HasValue) request.AddQueryParameter("created_at", input.CreatedAt.Value.ToString("yyyy-MM-dd"));
        if (input.UpdatedAfter.HasValue) request.AddQueryParameter("updated_after", input.UpdatedAfter.Value.ToString("yyyy-MM-dd"));
        if (input.UpdatedBefore.HasValue) request.AddQueryParameter("updated_before", input.UpdatedBefore.Value.ToString("yyyy-MM-dd"));
        if (input.UpdatedAt.HasValue) request.AddQueryParameter("updated_at", input.UpdatedAt.Value.ToString("yyyy-MM-dd"));

        var articles = await Client.GetPaginatedResults<Article>(request);

        return new ListArticlesResponse { Articles = articles };
    }

    [Action("Get article metadata", Description = "Get metadata information of a specific article")]
    public async Task<Article> GetArticle([ActionParameter] ArticleIdentifier article)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Get);
        var response = await Client.ExecuteWithHandling<SingleArticle>(request);
        return response.Article;
    }


    [Action("Create article", Description = "Create a new article")]
    public async Task<Article> CreateArticle(
        [ActionParameter] LocaleIdentifier locale,
        [ActionParameter] SectionIdentifier section,
        [ActionParameter] CreateArticleRequest input,
        [ActionParameter] NotifySubscribersRequest notify
    )
    {
        var request = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/sections/{section.Id}/articles", Method.Post);
        request.AddNewtonJson(new { notify_subscribers = notify.NotifySubscribers, article = input });
        var response = await Client.ExecuteWithHandling<SingleArticle>(request);
        return response.Article;
    }

    [Action("Update article metadata", Description = "Update an article. This action does not update translation properties such as the article's title, body, locale, or draft. Use 'Upload article content'")]
    public async Task<Article> UpdateArticle([ActionParameter] ArticleIdentifier article,
        [ActionParameter] UpdateArticleRequest input)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Put);
        request.AddNewtonJson(new { article = input });
        var response = await Client.ExecuteWithHandling<SingleArticle>(request);
        return response.Article;
    }

    [Action("Archive article", Description = "Archive an article")]
    public async Task DeleteArticle([ActionParameter] ArticleIdentifier article)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Delete);
        await Client.ExecuteWithHandling(request);
    }

    public async Task<Translation?> GetArticleTranslation([ActionParameter] ArticleIdentifier article, [ActionParameter] LocaleIdentifier locale)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/translations/{locale.Locale}", Method.Get);
        var response = await Client.ExecuteAsync<SingleTranslation>(request);
        return response.Data?.Translation;
    }

    public async Task DeleteArticleTranslation([ActionParameter] ArticleIdentifier article, [ActionParameter] LocaleIdentifier locale)
    {
        var translation = await GetArticleTranslation(article, locale);
        if (translation != null)
        {
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/translations/{locale.Locale}", Method.Delete);
            await Client.ExecuteWithHandling(request);
        }
    }

    [Action("Search article missing locales", Description = "Returns the locales that are missing for this article")]
    public async Task<MissingLocales> GetArticleMissingTranslations([ActionParameter] ArticleIdentifier article)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/translations/missing", Method.Get);
        return await Client.ExecuteWithHandling<MissingLocales>(request);
    }

    [Action("Add image to article content", Description = "Add an image at the bottom of an article")]
    public async Task<Translation> AddImageToArticle([ActionParameter] ArticleIdentifier article, [ActionParameter] LocaleIdentifier locale, [ActionParameter] ImageRequest input)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/translations/{locale.Locale}", Method.Get);
        var response = await Client.ExecuteWithHandling<SingleTranslation>(request);

        using var fileStream = await _fileManagementClient.DownloadAsync(input.File);
        var fileAttachment = await fileStream.GetByteData();

        var uploadRequest = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/attachments", Method.Post);
        uploadRequest.AddFile("file", fileAttachment, input.File.Name);
        uploadRequest.AddParameter("inline", "true");

        var attachmentResponse = await Client.ExecuteWithHandling<AttachmentUploadResponse>(uploadRequest);

        var newHtml = $"{response.Translation.Body}<p><img src=\"{attachmentResponse.Attachment.ContentUrl}\" alt=\"{attachmentResponse.Attachment.DisplayFileName}\"></p>";

        return await TranslateArticle(article, new TranslationRequest { Locale = locale.Locale, Body = newHtml });
    }

    public async Task<Translation> TranslateArticle([ActionParameter] ArticleIdentifier article, [ActionParameter] TranslationRequest input)
    {
        var missingLocales = await GetArticleMissingTranslations(article);
        var isLocaleMissing = missingLocales.Locales.Contains(input.Locale);
        var request = ZendeskRequest.CreateTranslationUpsertRequest(isLocaleMissing, $"articles/{article.Id}", input.Locale);
        request.AddNewtonJson(input.Convert(isLocaleMissing));
        var response = await Client.ExecuteWithHandling<SingleTranslation>(request);
        return response.Translation;
    }

    [Action("Download article content", Description = "Get the translatable content of an article as a file")]
    public async Task<FileResponse> GetArticleAsFile([ActionParameter] ArticleIdentifier article, [ActionParameter] LocaleIdentifier locale)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/translations/{locale.Locale}", Method.Get);
        var response = await Client.ExecuteWithHandling<SingleTranslation>(request);

        string htmlFile =
            $"<html><head><title>{response.Translation.Title}</title><meta name=\"{Constants.Constants.BlackbirdReferenceId}\" content=\"{article.Id}\"></head><body>{response.Translation.Body}</body></html>";

        var invalidChars = new List<char>();
        invalidChars.AddRange(Path.GetInvalidFileNameChars());
        invalidChars.AddRange(new char[] { '?', '"', '<', '>', '/', '\\', ':', '|', '*' });
        var removeInvalidChars = new Regex($"[{Regex.Escape(new string(invalidChars.ToArray()))}]", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        var filename = removeInvalidChars.Replace(response.Translation.Title, "");

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(htmlFile));
        var file = await _fileManagementClient.UploadAsync(stream, MediaTypeNames.Text.Html, $"{filename}.html");
        return new FileResponse { File = file };
    }


    [Action("Get article ID from content file", Description = "Return the article ID embedded in the content file")]
    public async Task<ArticleIdResponse> GetArticleIdFromHtmlFile([ActionParameter] FileRequest file)
    {
        using var stream = await _fileManagementClient.DownloadAsync(file.File);
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        var fileBytes = ms.ToArray();

        var referenceId = FileTranslationRequest.ExtractBlackbirdId(fileBytes);

        if (string.IsNullOrWhiteSpace(referenceId))
            throw new PluginMisconfigurationException("No ID was found in metadata.");

        return new ArticleIdResponse { ArticleId = referenceId };
    }

    [Action("Upload article content", Description ="Updates the translation for an article, creates a new translation if there is none. Takes a translated file (HTML) as input.")]
    public async Task<Translation> TranslateArticleFromFile(
        [ActionParameter] ArticleOptimalIdentifier article,
        [ActionParameter] FileTranslationRequest input
        )
    {
        if (string.IsNullOrWhiteSpace(input.Locale))
        {
            throw new PluginMisconfigurationException("Locale value is empty. Please provide a valid locale and try again.");
        }

        var fileReference = await _fileManagementClient.DownloadAsync(input.File);
        var fileBytes = await fileReference.GetByteData();
        
        var articleId = article.Id ?? FileTranslationRequest.ExtractBlackbirdId(fileBytes) ?? throw new PluginMisconfigurationException("Blackbird reference ID not found in the HTML file and no article ID provided. Did you use a content file downloaded through Blackbird?");
        var articleRequest = new ArticleIdentifier { Id = articleId };
        
        var missingLocales = await GetArticleMissingTranslations(articleRequest);
        var isLocaleMissing = missingLocales.Locales.Contains(input.Locale);
        var converted = input.Convert(fileBytes, isLocaleMissing);

        var request =
            ZendeskRequest.CreateTranslationUpsertRequest(isLocaleMissing, $"articles/{articleId}", input.Locale);
        request.AddNewtonJson(converted);
        var response = await Client.ExecuteWithRetries<SingleTranslation>(request);
        return response.Translation;
    }

    [Action("Add label to article", Description = "Add a new label to an article")]
    public async Task AddArticleLabel([ActionParameter] ArticleIdentifier article, [ActionParameter] LocaleIdentifier locale, [ActionParameter][Display("Name")] string name)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/articles/{article.Id}/labels", Method.Post);
        request.AddJsonBody(new { label = new { name = name } });
        await Client.ExecuteWithHandling(request);
    }

    [Action("Delete label from article", Description = "Delete a label from an article")]
    public async Task DeleteArticleLabel([ActionParameter] ArticleIdentifier article, [ActionParameter] LocaleIdentifier locale, [ActionParameter, Display("Label ID"), DataSource(typeof(LabelDataHandler))] string labelId)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/articles/{article.Id}/labels/{labelId}", Method.Delete);
        await Client.ExecuteWithHandling(request);
    }
}