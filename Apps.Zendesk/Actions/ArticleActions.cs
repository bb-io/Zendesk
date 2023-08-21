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
using System.Text.Json;
using Newtonsoft.Json;

namespace Apps.Zendesk.Actions
{
    [ActionList]
    public class ArticleActions : BaseInvocable
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        public ArticleActions(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        [Action("Get all articles", Description = "Get all articles that have changed recently, optionally those that are missing translations")]
        public async Task<List<Article>> GetAllSections([ActionParameter] HoursIdentifier hours, [ActionParameter] OptionalMissingLocaleIdentifier missingLocalesInput, [ActionParameter] OptionalCategoryIdentifier category)
        {
            var client = new ZendeskClient(Creds);

            List<Article> articles = new List<Article>();

            var targetTime = DateTime.UtcNow.AddHours(-1 * hours.Hours);
            var unixTime = ((DateTimeOffset)targetTime).ToUnixTimeSeconds();

            if (category.Id != null)
            {
                var endpoint = $"/api/v2/help_center/categories/{category.Id}/articles";
                var request = new ZendeskRequest(endpoint, Method.Get, Creds);
                var response = client.GetPaginated<MultipleArticles>(request).SelectMany(x => x.Articles);
                articles.AddRange(response.Where(x => x.UpdatedAt >= targetTime));
            } else
            {
                var endpoint = $"/api/v2/help_center/incremental/articles?start_time={unixTime}";
                var request = new ZendeskRequest("/api/v2/help_center/incremental/articles", Method.Get, Creds);
                request.AddQueryParameter("start_time", unixTime);
                request.AddQueryParameter("per_page", 100);
                var response = client.GetPaginated<MultipleArticles>(request).SelectMany(x => x.Articles);
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
        public async Task<Article> GetArticle([ActionParameter] ArticleIdentifier article)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Get, Creds);
            var response = await client.ExecuteWithHandling<SingleArticle>(request);
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
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/sections/{section.Id}/articles", Method.Post, Creds);
            request.AddNewtonJson(new { notify_subscribers = notify.NotifySubscribers, article = input });
            var response = await client.ExecuteWithHandling<SingleArticle>(request);
            return response.Article;
        }

        [Action("Update article", Description = "Update an article")]
        public async Task<Article> UpdateArticle([ActionParameter] ArticleIdentifier article, [ActionParameter] UpdateArticleRequest input)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Put, Creds);
            request.AddNewtonJson(new { article = input });
            var response = await client.ExecuteWithHandling<SingleArticle>(request);
            return response.Article;
        }

        [Action("Archive article", Description = "Archive an article")]
        public async Task DeleteArticle([ActionParameter] ArticleIdentifier article)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Delete, Creds);
            await client.ExecuteWithHandling(request);
        }

        [Action("Get all article locales", Description = "Get all existing locales of this article")]
        public async Task<List<Translation>> GetArticleTranslations([ActionParameter] ArticleIdentifier article)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/translations", Method.Get, Creds);
            var translations = client.GetPaginated<MultipleTranslations>(request).SelectMany(x => x.Translations);
            return translations.ToList();
        }

        [Action("Get article translation", Description = "Get the translation of an article for a specific locale")]
        public async Task<Translation> GetArticleTranslation([ActionParameter] ArticleIdentifier articles, [ActionParameter] LocaleIdentifier locale)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{articles.Id}/translations/{locale.Locale}", Method.Get, Creds);
            var response = await client.ExecuteWithHandling<SingleTranslation>(request);
            return response.Translation;
        }

        [Action("Get article missing translations", Description = "Get the locales that are missing for this article")]
        public async Task<MissingLocales> GetArticleMissingTranslations([ActionParameter] ArticleIdentifier article)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}/translations/missing", Method.Get, Creds);
            return await client.ExecuteWithHandling<MissingLocales>(request);
        }

        [Action("Update article translation", Description = "Updates the translation for an article, creates a new translation if there is none")]
        public async Task<Translation> TranslateArticle([ActionParameter] ArticleIdentifier article, [ActionParameter] TranslationRequest input)
        {
            var client = new ZendeskClient(Creds);
            var missingLocales = await GetArticleMissingTranslations(article);
            var isLocaleMissing = missingLocales.Locales.Contains(input.Locale);
            var request = ZendeskRequest.CreateTranslationUpsertRequest(isLocaleMissing, $"articles/{article.Id}", input.Locale, Creds);
            request.AddNewtonJson(input.Convert(isLocaleMissing));
            var response = await client.ExecuteWithHandling<SingleTranslation>(request);
            return response.Translation;
        }


        [Action("Get article as HTML file", Description = "Get the translatable content of an article as a file")]
        public async Task<FileResponse> GetArticleAsFile([ActionParameter] ArticleIdentifier article)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{article.Id}", Method.Get, Creds);
            var response = await client.ExecuteWithHandling<SingleArticle>(request);

            string htmlFile = $"<html><head><title>{response.Article.Title}</title></head><body>{response.Article.Body}</body></html>";

            return new FileResponse()
            {
                File = Encoding.UTF8.GetBytes(htmlFile)
            };
        }

        [Action("Update article translation from HTML file",
            Description = "Updates the translation for an article, creates a new translation if there is none. Takes a translated HTML file as input")]
        public async Task<Translation> TranslateArticleFromFile([ActionParameter] ArticleIdentifier article, [ActionParameter] FileTranslationRequest input)
        {
            var client = new ZendeskClient(Creds);
            var missingLocales = await GetArticleMissingTranslations(article);
            var isLocaleMissing = missingLocales.Locales.Contains(input.Locale);
            var request = ZendeskRequest.CreateTranslationUpsertRequest(isLocaleMissing, $"articles/{article.Id}", input.Locale, Creds);
            request.AddNewtonJson(input.Convert(isLocaleMissing));
            var response = await client.ExecuteWithHandling<SingleTranslation>(request);
            return response.Translation;
        }

    }
}