using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.OpenAI.Models.Responses;
using Apps.Zendesk.Models.Requests;
using RestSharp;
using System.Text;
using Apps.Zendesk.Models.Responses;
using Newtonsoft.Json;
using HtmlAgilityPack;
using Blackbird.Applications.Sdk.Common.Actions;
using Apps.Zendesk.Dtos;
using System.Linq;

namespace Apps.Zendesk.Actions
{
    [ActionList]
    public class ArticleActions
    {
        [Action("Get all articles", Description = "List all articles")]
        public ListArticlesResponse ListArticles(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/articles",
                Method.Get, authenticationCredentialsProviders);
            return new ListArticlesResponse()
            {
                Articles = client.Get<ArticlesResponseWrapper>(request)?.Articles.Select(Article.FromDto).Where(x => x != null) ?? new List<Article>()
            };
        }

        [Action("Get changed articles", Description = "Get all articles that have been created or edited in the last x hours")]
        public ListArticlesResponse GetChangedArticles(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, [ActionParameter] ChangedArticlesRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/articles",
                Method.Get, authenticationCredentialsProviders);
            var articles = client.Get<ArticlesResponseWrapper>(request)?.Articles.Select(Article.FromDto).Where(x => x != null) ?? new List<Article>();
            var moment = DateTime.Now.AddHours(-1 * input.Hours);
            return new ListArticlesResponse()
            {
                Articles = articles.Where(x => x.CreatedAt > moment || x.EditedAt > moment || x.UpdatedAt > moment)
            };
        }

        [Action("Get articles by language", Description = "List all articles by language")]
        public ListArticlesResponse ListArticlesByLanguage(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListArticlesRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/{input.Locale}/articles", 
                Method.Get, authenticationCredentialsProviders);
            return new ListArticlesResponse()
            {
                Articles = client.Get<ArticlesResponseWrapper>(request)?.Articles.Select(Article.FromDto) ?? new List<Article>()
            };
        }

        [Action("Get articles not translated in language", Description = "Get articles not translated in specific language")]
        public ListArticlesResponse GetArticlesNotTranslated(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter][Display("Locale")] string locale)
        {
            var allArticles = ListArticles(authenticationCredentialsProviders).Articles;
            var allTranslatedArticles = ListArticlesByLanguage(authenticationCredentialsProviders, new ListArticlesRequest() { Locale = locale }).Articles;
            return new ListArticlesResponse()
            {
                Articles = allArticles.Where(a1 => !allTranslatedArticles.Any(a2 => a2.Id == a1.Id)).ToList()
            };
        }

        [Action("Get article", Description = "Get article by Id")]
        public Article? GetArticleById(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetArticleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/{input.Locale}/articles/{input.ArticleId}",
                Method.Get, authenticationCredentialsProviders);
            return Article.FromDto(client.Get<ArticleResponseWrapper<ArticleDto>>(request)?.Article);
        }

        [Action("Get article as HTML file", Description = "Get the translatable content of an article as a file")]
        public FileResponse GetArticleAsFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] GetArticleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/{input.Locale}/articles/{input.ArticleId}",
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
                File = Encoding.ASCII.GetBytes(htmlFile)
            };
        }

        [Action("Translate article from HTML file", Description = "Create a new translation for an article based on a file input")]
        public void TranslateArticleFromFile(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TranslateArticleFromFileRequest input)
        {
            var notTranslated = GetArticlesNotTranslated(authenticationCredentialsProviders, input.Locale);
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var translationDoesNotExist = notTranslated.Articles.Any(a => a.Id == input.ArticleId);
            var request = translationDoesNotExist ?
                new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations", Method.Post, authenticationCredentialsProviders) :
                new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations/{input.Locale}", Method.Put, authenticationCredentialsProviders);

            var fileString = Encoding.ASCII.GetString(input.File);
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
                    body = body
                }
            });
            var response = client.Execute(request);

            if (!response.IsSuccessful)
                throw new Exception(response.Content);
        }

        [Action("Create or update article translation", Description = "Create or update article translation")]
        public void CreateOrUpdateTranslation(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TranslateArticleRequest input)
        {
            var notTranslated = GetArticlesNotTranslated(authenticationCredentialsProviders, input.Locale);
            if(notTranslated.Articles.Any(a => a.Id == input.ArticleId))
            {
                TranslateArticle(authenticationCredentialsProviders, input);
            }
            else
            {
                UpdateArticleTranslation(authenticationCredentialsProviders, input);
            }
        }

        //[Action("Create article translation (new)", Description = "Create a new translation for an article")]
        private void TranslateArticle(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TranslateArticleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations",
                Method.Post, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                translation = new
                {
                    locale = input.Locale,
                    title = input.Title,
                    body = input.Body
                }
            });

            try
            {
                client.Execute(request);
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                {
                    throw new Exception("Specified language is not supported in your Zendesk. Please add it in language settings");
                }
            }
        }

        //[Action("Update article translation", Description = "Update an existing article translation")]
        private void UpdateArticleTranslation(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] TranslateArticleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations/{input.Locale}",
                Method.Put, authenticationCredentialsProviders);
            request.AddJsonBody(new
            {
                translation = new
                {
                    title = input.Title,
                    body = input.Body
                }
            });
            client.Execute(request);
        }
    }
}
