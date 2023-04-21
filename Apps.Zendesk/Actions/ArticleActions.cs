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

namespace Apps.Zendesk.Actions
{
    [ActionList]
    public class ArticleActions
    {
        [Action("List articles", Description = "List all articles")]
        public ListArticlesResponse ListArticles(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
            [ActionParameter] ListArticlesRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new ZendeskRequest($"/api/v2/help_center/{input.Locale}/articles", 
                Method.Get, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization"));
            return new ListArticlesResponse()
            {
                Articles = client.Get<ArticlesResponseWrapper>(request).Articles
            };
        }

        [Action("Get article", Description = "Get article by Id")]
        public ArticleDto GetArticleById(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders, string yourUrl,
            [ActionParameter] GetArticleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new ZendeskRequest($"/api/v2/help_center/{input.Locale}/articles/{input.ArticleId}",
                Method.Get, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization"));
            return client.Get<ArticleResponseWrapper<ArticleDto>>(request).Article;
        }

        [Action("Get article as HTML file", Description = "Get the translatable content of an article as a file")]
        public FileResponse GetArticleAsFile(string url, string email, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetArticleRequest input)
        {
            var articlesListEndpoint = $"/api/v2/help_center/{input.Locale}/articles/{input.ArticleId}";
            var request = CreateRequestToZendesk(email, authenticationCredentialsProvider.Value, articlesListEndpoint, Method.Get);
            var response = new RestClient(url).Get(request);

            dynamic parsedArticle = JsonConvert.DeserializeObject(response.Content);

            string title = parsedArticle.article.title;
            string body = parsedArticle.article.body;

            string htmlFile = $"<html><head><title>{title}</title></head><body>{body}</body>";

            return new FileResponse()
            {
                File = Encoding.ASCII.GetBytes(htmlFile)
            };
        }

        [Action("Create article translation", Description = "Create a new translation for an article")]
        public BaseResponse TranslateArticle(string url, string email, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] TranslateArticleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations",
                Method.Post, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization"));
            request.AddJsonBody(new
            {
                translation = new
                {
                    locale = input.Locale,
                    title = input.TranslatedTitle,
                    body = input.TranslatedBody
                }
            });
            client.Execute(request);
        }

        [Action("Translate article from HTML file", Description = "Create a new translation for an article based on a file input")]
        public BaseResponse TranslateArticleFromFile(string url, string email, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] TranslateArticleFromFileRequest input)
        {
            var articlesListEndpoint = $"/api/v2/help_center/articles/{input.ArticleId}/translations";
            var request = CreateRequestToZendesk(email, authenticationCredentialsProvider.Value, articlesListEndpoint, Method.Post);

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
            var response = new RestClient(url).Execute(request);

            return new BaseResponse()
            {
                StatusCode = ((int)response.StatusCode),
                Details = response.Content
            };
        }

        [Action("Update article translation", Description = "Update an existing article translation")]
        public BaseResponse UpdateArticleTranslation(string url, string email, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] TranslateArticleRequest input)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{input.ArticleId}/translations/{input.Locale}",
                Method.Put, authenticationCredentialsProviders.First(p => p.KeyName == "Authorization"));
            request.AddJsonBody(new
            {
                translation = new
                {
                    title = input.TranslatedTitle,
                    body = input.TranslatedBody
                }
            });
            client.Execute(request);
        }
    }
}
