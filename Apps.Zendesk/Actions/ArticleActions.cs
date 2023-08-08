using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.Zendesk.Models.Requests;
using RestSharp;
using System.Text;
using Apps.Zendesk.Models.Responses;
using Newtonsoft.Json;
using HtmlAgilityPack;
using Blackbird.Applications.Sdk.Common.Actions;
using Apps.Zendesk.Dtos;

namespace Apps.Zendesk.Actions
{
    [ActionList]
    public class ArticleActions
    {
        [Action("Get all articles", Description = "List all articles")]
        public ListArticlesResponse ListArticles(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListArticlesRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);

            var endpoint = "/api/v2/help_center/articles";
            if (input.Hours != null)
            {
                var currentTime = DateTime.UtcNow.AddHours(-1 * (int)input.Hours);
                var unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();
                endpoint = $"/api/v2/help_center/incremental/articles?start_time={unixTime}";
            }

            var request = new ZendeskRequest(endpoint,
                Method.Get, authenticationCredentialsProviders);
            var results = client.GetPaginated<ArticlesResponseWrapper>(request);
            var articles = results.SelectMany(x => x.Articles).Select(Article.FromDto).Where(x => x != null);

            return new ListArticlesResponse
            {
                Articles = articles
            };
        }

        [Action("Get articles not translated in language",
            Description = "Get articles not translated in specific language")]
        public ListArticlesResponse GetArticlesNotTranslated(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListArticlesWithLocaleRequest input)
        {
            var allArticles = ListArticles(authenticationCredentialsProviders,
                new ListArticlesRequest { Hours = input.Hours }).Articles;
            var articlesWithMissingLocales = new List<Article>();

            foreach (var article in allArticles)
            {
                var missingLocales = GetArticleMissingTranslations(authenticationCredentialsProviders,
                    new GetMissingLocaleRequest { ArticleId = article.Id })?.Locales;
                if (missingLocales.Contains(input.Locale))
                    articlesWithMissingLocales.Add(article);
            }

            return new ListArticlesResponse()
            {
                Articles = articlesWithMissingLocales
            };
        }

        [Action("Get article", Description = "Get information on a specific article")]
        public Article? GetArticleById(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetArticleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var endpoint = input.Locale == null
                ? $"/api/v2/help_center/articles/{input.ArticleId}"
                : $"/api/v2/help_center/{input.Locale}/articles/{input.ArticleId}";
            var request = new ZendeskRequest(endpoint,
                Method.Get, authenticationCredentialsProviders);
            return Article.FromDto(client.Get<ArticleResponseWrapper<ArticleDto>>(request)?.Article);
        }

        [Action("Get article missing translations", Description = "Get the locales that are missing for this article")]
        public LocalesResponse? GetArticleMissingTranslations(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetMissingLocaleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations/missing",
                Method.Get, authenticationCredentialsProviders);
            return client.Get<LocalesResponse>(request);
        }

        [Action("Get article translations", Description = "Get all translations for an article")]
        public TranslationsResponse? GetArticleTranslations(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetArticleTranslationsRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations",
                Method.Get, authenticationCredentialsProviders);

            var results = client.GetPaginated<TranslationsResponseWrapper>(request);
            var translations = results.SelectMany(x => x.Translations).Select(Translation.FromDto)
                .Where(x => x != null);

            return new TranslationsResponse
            {
                Translations = translations
            };
        }

        [Action("Get article as HTML file", Description = "Get the translatable content of an article as a file")]
        public FileResponse GetArticleAsFile(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetArticleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var endpoint = input.Locale == null
                ? $"/api/v2/help_center/articles/{input.ArticleId}"
                : $"/api/v2/help_center/{input.Locale}/articles/{input.ArticleId}";
            var request = new ZendeskRequest(endpoint,
                Method.Get, authenticationCredentialsProviders);
            var response = client.Get(request);

            if (!response.IsSuccessful)
                throw new Exception(response.Content);

            dynamic parsedArticle = JsonConvert.DeserializeObject(response.Content);

            string title = parsedArticle.article.title;
            string body = parsedArticle.article.body;

            string htmlFile = $"<html><head><title>{title}</title></head><body>{body}</body></html>";

            return new FileResponse()
            {
                File = Encoding.UTF8.GetBytes(htmlFile)
            };
        }

        [Action("Translate article from HTML file",
            Description = "Create a new translation for an article based on a file input")]
        public void TranslateArticleFromFile(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TranslateArticleFromFileRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var missingLocales = GetArticleMissingTranslations(authenticationCredentialsProviders,
                new GetMissingLocaleRequest { ArticleId = input.ArticleId })?.Locales;
            var request = missingLocales.Contains(input.Locale)
                ? new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations", Method.Post,
                    authenticationCredentialsProviders)
                : new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations/{input.Locale}",
                    Method.Put, authenticationCredentialsProviders);

            var fileString = Encoding.UTF8.GetString(input.File);
            var doc = new HtmlDocument();
            doc.LoadHtml(fileString);
            var title = doc.DocumentNode.SelectSingleNode("html/head/title").InnerText;
            var body = doc.DocumentNode.SelectSingleNode("/html/body").InnerHtml;

            request.AddJsonBody(new
            {
                translation = new
                {
                    locale = input.Locale,
                    title = title,
                    body = body,
                    draft = input.Draft
                }
            });
            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new Exception(response.Content);
        }

        [Action("Translate article", Description = "Create or update an article translation")]
        public void CreateOrUpdateTranslation(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TranslateArticleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var missingLocales = GetArticleMissingTranslations(authenticationCredentialsProviders,
                new GetMissingLocaleRequest { ArticleId = input.ArticleId })?.Locales;
            var request = missingLocales.Contains(input.Locale)
                ? new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations", Method.Post,
                    authenticationCredentialsProviders)
                : new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations/{input.Locale}",
                    Method.Put, authenticationCredentialsProviders);

            request.AddJsonBody(new
            {
                translation = new
                {
                    locale = input.Locale,
                    title = input.Title,
                    body = input.Body,
                    draft = input.Draft
                }
            });
            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new Exception(response.Content);
        }
    }
}