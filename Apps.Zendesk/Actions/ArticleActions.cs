using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Apps.OpenAI.Models.Responses;
using Apps.Zendesk.Models.Requests;
using RestSharp;
using System.Text;
using Apps.Zendesk.Models.Responses;
using Newtonsoft.Json;
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

        [Action("Translate article", Description = "Translate article by Id")]
        public void TranslateArticle(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
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

        [Action("Update translation", Description = "Update translation for the article")]
        public void UpdateArticleTranslation(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
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
