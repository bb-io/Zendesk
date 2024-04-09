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

namespace Apps.Zendesk.Actions;

[ActionList]
public class ArticleActions : BaseInvocable
{
    private readonly IFileManagementClient _fileManagementClient;
    
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private ZendeskClient Client { get; }

    public ArticleActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) 
        : base(invocationContext)
    {
        _fileManagementClient = fileManagementClient;
        Client = new ZendeskClient(invocationContext);
    }

    [Action("Get all articles",
        Description =
            "Get all articles that have changed recently, optionally those that are missing translations")]
    public async Task<List<Article>> GetAllArticles([ActionParameter] HoursIdentifier hours,
        [ActionParameter] OptionalMissingLocaleIdentifier missingLocalesInput,
        [ActionParameter] OptionalCategoryIdentifier category)
    {
        List<Article> articles = new List<Article>();

        var targetTime = DateTime.UtcNow.AddHours(-1 * hours.Hours);
        var unixTime = ((DateTimeOffset)targetTime).ToUnixTimeSeconds();

        if (category.Id != null)
        {
            var endpoint = $"/api/v2/help_center/categories/{category.Id}/articles";
            var request = new ZendeskRequest(endpoint, Method.Get, Creds);
            var response = Client.GetPaginated<MultipleArticles>(request).SelectMany(x => x.Articles);
            articles.AddRange(response.Where(x => x.UpdatedAt >= targetTime));
        }
        else
        {
            var request = new ZendeskRequest("/api/v2/help_center/incremental/articles", Method.Get, Creds);
            request.AddQueryParameter("start_time", unixTime);
            request.AddQueryParameter("per_page", 100);
            var response = Client.GetPaginated<MultipleArticles>(request).SelectMany(x => x.Articles);
            articles.AddRange(response);
        }

        if (missingLocalesInput.Locale != null)
        {
            var filteredArticles = new List<Article>();
            foreach (var article in articles)
            {
                var missingLocales = await GetArticleMissingTranslations(new ArticleIdentifier { Id = article.Id });
                if (missingLocales.Locales.Contains(missingLocalesInput.Locale))
                    filteredArticles.Add(article);
            }

            articles = filteredArticles;
        }

        return articles.ToList();
    }

    [Action("Get article", Description = "Get information on a specific article")]
    public async Task<ArticleWithLabels> GetArticle([ActionParameter] ArticleIdentifier article)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Get, Creds);
        var response = await Client.ExecuteWithHandling<SingleArticle>(request);
        var labels = new List<string>();

        try
        {
            var labelRequest = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/labels", Method.Get, Creds);
            var labelResponse = await Client.ExecuteWithHandling<LabelResponse>(labelRequest);
            labels = labelResponse.Labels.Select(x => x.Name).ToList();
        }
        catch { }
        return new(response.Article, labels);
    }


    [Action("Create article", Description = "Create a new article")]
    public async Task<Article> CreateArticle(
        [ActionParameter] LocaleIdentifier locale,
        [ActionParameter] SectionIdentifier section,
        [ActionParameter] CreateArticleRequest input,
        [ActionParameter] NotifySubscribersRequest notify
    )
    {
        var request = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/sections/{section.Id}/articles",
            Method.Post, Creds);
        request.AddNewtonJson(new { notify_subscribers = notify.NotifySubscribers, article = input });
        var response = await Client.ExecuteWithHandling<SingleArticle>(request);
        return response.Article;
    }

    [Action("Update article", Description = "Update an article. This action does not update translation properties such as the article's title, body, locale, or draft. Use 'Update article translation'")]
    public async Task<Article> UpdateArticle([ActionParameter] ArticleIdentifier article,
        [ActionParameter] UpdateArticleRequest input)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Put, Creds);
        request.AddNewtonJson(new { article = input });
        var response = await Client.ExecuteWithHandling<SingleArticle>(request);
        return response.Article;
    }

    [Action("Archive article", Description = "Archive an article")]
    public async Task DeleteArticle([ActionParameter] ArticleIdentifier article)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Delete, Creds);
        await Client.ExecuteWithHandling(request);
    }

    [Action("Get all article translations", Description = "Get all existing translations of this article")]
    public async Task<List<Translation>> GetArticleTranslations([ActionParameter] ArticleIdentifier article)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/translations", Method.Get,
            Creds);
        var translations = Client.GetPaginated<MultipleTranslations>(request).SelectMany(x => x.Translations);
        return translations.ToList();
    }

    [Action("Get article translation", Description = "Get the translation of an article for a specific locale")]
    public async Task<TranslationWithLabels> GetArticleTranslation([ActionParameter] ArticleIdentifier article,
        [ActionParameter] LocaleIdentifier locale)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/translations/{locale.Locale}",
            Method.Get, Creds);
        var response = await Client.ExecuteWithHandling<SingleTranslation>(request);

        var labels = new List<string>();

        try
        {
            var labelRequest = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/articles/{article.Id}/labels", Method.Get, Creds);
            var labelResponse = await Client.ExecuteWithHandling<LabelResponse>(labelRequest);
            labels = labelResponse.Labels.Select(x => x.Name).ToList();
        }
        catch { }

        return new(response.Translation, labels);
    }

    [Action("Get article missing translations", Description = "Get the locales that are missing for this article")]
    public async Task<MissingLocales> GetArticleMissingTranslations([ActionParameter] ArticleIdentifier article)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/translations/missing",
            Method.Get, Creds);
        return await Client.ExecuteWithHandling<MissingLocales>(request);
    }

    [Action("Add image to article", Description = "Add an image to the bottom of the article")]
    public async Task<Translation> AddImageToArticle([ActionParameter] ArticleIdentifier article, [ActionParameter] LocaleIdentifier locale, 
            [ActionParameter] ImageRequest input)
    {
        var articleResponse = await GetArticleTranslation(article, locale);

        using var fileStream = await _fileManagementClient.DownloadAsync(input.File);
        var fileAttachment = await fileStream.GetByteData();

        var uploadRequest = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/attachments", Method.Post, Creds);
        uploadRequest.AddFile("file", fileAttachment, input.File.Name);
        uploadRequest.AddParameter("inline", "true");

        var attachmentResponse = await Client.ExecuteWithHandling<AttachmentUploadResponse>(uploadRequest);

        var newHtml = $"{articleResponse.Body}<p><img src=\"{attachmentResponse.Attachment.ContentUrl}\" alt=\"{attachmentResponse.Attachment.DisplayFileName}\"></p>";

        return await TranslateArticle(article, new TranslationRequest { Locale = locale.Locale, Body = newHtml });
    }

    [Action("Update article translation",
        Description = "Updates the translation for an article, creates a new translation if there is none")]
    public async Task<Translation> TranslateArticle([ActionParameter] ArticleIdentifier article,
        [ActionParameter] TranslationRequest input)
    {
        var missingLocales = await GetArticleMissingTranslations(article);
        var isLocaleMissing = missingLocales.Locales.Contains(input.Locale);
        var request =
            ZendeskRequest.CreateTranslationUpsertRequest(isLocaleMissing, $"articles/{article.Id}", input.Locale,
                Creds);
        request.AddNewtonJson(input.Convert(isLocaleMissing));
        var response = await Client.ExecuteWithHandling<SingleTranslation>(request);
        return response.Translation;
    }


    [Action("Get article as HTML file", Description = "Get the translatable content of an article as a file")]
    public async Task<FileResponse> GetArticleAsFile([ActionParameter] ArticleIdentifier article)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Get, Creds);
        var response = await Client.ExecuteWithHandling<SingleArticle>(request);

        string htmlFile =
            $"<html><head><title>{response.Article.Title}</title></head><body>{response.Article.Body}</body></html>";

        var invalidChars = new List<char>();
        invalidChars.AddRange(Path.GetInvalidFileNameChars());
        invalidChars.AddRange(new char[] { '?', '"', '<', '>', '/', '\\', ':', '|', '*' });
        var removeInvalidChars = new Regex($"[{Regex.Escape(new string(invalidChars.ToArray()))}]", RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
        var filename = removeInvalidChars.Replace(response.Article.Name, "");

        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(htmlFile));
        var file = await _fileManagementClient.UploadAsync(stream, MediaTypeNames.Text.Html, $"{filename}.html");
        return new FileResponse { File = file };
    }

    [Action("Update article translation from HTML file",
        Description =
            "Updates the translation for an article, creates a new translation if there is none. Takes a translated HTML file as input")]
    public async Task<Translation> TranslateArticleFromFile([ActionParameter] ArticleIdentifier article,
        [ActionParameter] FileTranslationRequest input)
    {
        var missingLocales = await GetArticleMissingTranslations(article);
        var isLocaleMissing = missingLocales.Locales.Contains(input.Locale);
        var request =
            ZendeskRequest.CreateTranslationUpsertRequest(isLocaleMissing, $"articles/{article.Id}", input.Locale,
                Creds);
        request.AddNewtonJson(input.Convert(isLocaleMissing, _fileManagementClient));
        var response = await Client.ExecuteWithHandling<SingleTranslation>(request);
        return response.Translation;
    }

    [Action("Add label to article", Description = "Add a new label to an article")]
    public async Task AddArticleLabel([ActionParameter] ArticleIdentifier article, [ActionParameter] LocaleIdentifier locale, [ActionParameter][Display("Name")] string name)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/articles/{article.Id}/labels", Method.Post, Creds);
        request.AddJsonBody(new { label = new { name = name } });
        await Client.ExecuteWithHandling(request);
    }

    [Action("Delete label from article", Description = "Delete a label from an article")]
    public async Task DeleteArticleLabel([ActionParameter] ArticleIdentifier article, [ActionParameter] LocaleIdentifier locale, [ActionParameter][Display("Label")][DataSource(typeof(LabelDataHandler))] string labelId)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/articles/{article.Id}/labels/{labelId}", Method.Delete, Creds);
        await Client.ExecuteWithHandling(request);
    }
}