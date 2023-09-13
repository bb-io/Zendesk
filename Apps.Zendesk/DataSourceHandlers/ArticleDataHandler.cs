using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class ArticleDataHandler : BaseInvocable, IDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public ArticleDataHandler(InvocationContext invocationContext) : base(invocationContext) {}

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var client = new ZendeskClient(InvocationContext);
        IEnumerable<Article> articles;
        if (string.IsNullOrEmpty(context.SearchString))
        {
            var request = new ZendeskRequest("/api/v2/help_center/articles", Method.Get, Creds);
            articles = client.Execute<MultipleArticles>(request).Articles;
        } else
        {
            var request = new ZendeskRequest("/api/v2/help_center/articles/search", Method.Get, Creds);
            request.AddQueryParameter("query", context.SearchString);
            articles = client.Execute<SearchResponse<Article>>(request).Results;
        }

        return articles
            .OrderByDescending(x => x.UpdatedAt)
            .ToDictionary(x => x.Id.ToString(), x => x.Title);
    }
}