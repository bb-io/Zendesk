using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.OpenAI.Models.Responses;
using Apps.Zendesk.Models.Requests;
using RestSharp;
using System.Buffers.Text;
using System.Text;
using Apps.Zendesk.Models.Responses;
using Newtonsoft.Json;
using HtmlAgilityPack;

namespace Apps.Zendesk.Actions
{
    [ActionList]
    public class ArticleActions
    {
        [Action("List all articles", Description = "Get a list of all articles on this Zendesk")]
        public ListArticlesResponse ListArticles(string url, string email, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] ListArticlesRequest input)
        {            
            var articlesListEndpoint = $"/api/v2/help_center/{input.Locale}/articles";
            var request = CreateRequestToZendesk(email, authenticationCredentialsProvider.Value, articlesListEndpoint, Method.Get);
            var response = new RestClient(url).Get(request);
           
            return new ListArticlesResponse()
            {
                Articles = response.Content
            };
        }

        [Action("Get article",  Description = "Get a specific article by ID")]
        public GetArticleResponse GetArticleById(string url, string email, AuthenticationCredentialsProvider authenticationCredentialsProvider,
            [ActionParameter] GetArticleRequest input)
        {
            var articlesListEndpoint = $"/api/v2/help_center/{input.Locale}/articles/{input.ArticleId}";
            var request = CreateRequestToZendesk(email, authenticationCredentialsProvider.Value, articlesListEndpoint, Method.Get);
            var response = new RestClient(url).Get(request);

            dynamic parsedArticle = JsonConvert.DeserializeObject(response.Content);

            string title = parsedArticle.article.title;
            string body = parsedArticle.article.body;

            return new GetArticleResponse()
            {
                Title = title,
                Body = body
            };
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
            var articlesListEndpoint = $"/api/v2/help_center/articles/{input.ArticleId}/translations";
            var request = CreateRequestToZendesk(email, authenticationCredentialsProvider.Value, articlesListEndpoint, Method.Post);
            request.AddJsonBody(new
            {
                translation = new
                {
                    locale = input.Locale,
                    title = input.TranslatedTitle,
                    body = input.TranslatedBody
                }
            });
            var response = new RestClient(url).Execute(request);
            
            return new BaseResponse()
            {
                StatusCode = ((int)response.StatusCode),
                Details = response.Content
            };
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
            var articlesListEndpoint = $"/api/v2/help_center/articles/{input.ArticleId}/translations/{input.Locale}";
            var request = CreateRequestToZendesk(email, authenticationCredentialsProvider.Value, articlesListEndpoint, Method.Put);
            request.AddJsonBody(new
            {
                translation = new
                {
                    title = input.TranslatedTitle,
                    body = input.TranslatedBody
                }
            });
            var response = new RestClient(url).Execute(request);

            return new BaseResponse()
            {
                StatusCode = ((int)response.StatusCode),
                Details = response.Content
            };
        }

        private RestRequest CreateRequestToZendesk(string email, string token, string endpoint,
            RestSharp.Method methodType)
        {
            string base64Key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{email}/token:{token}"));
            var request = new RestRequest(endpoint, methodType);
            request.AddHeader("Authorization", $"Basic {base64Key}");
            return request;
        }
    }
}
