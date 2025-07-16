using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class ArticleDataHandler : BaseInvocable, IAsyncDataSourceItemHandler
{
    public ArticleDataHandler(InvocationContext invocationContext) : base(invocationContext) {}

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(InvocationContext);
        IEnumerable<Article> articles;
        if (string.IsNullOrEmpty(context.SearchString))
        {
            var request = new ZendeskRequest("/api/v2/help_center/articles", Method.Get);
            articles = (await client.ExecuteWithHandling<MultipleArticles>(request)).Articles;
        } else
        {
            var request = new ZendeskRequest("/api/v2/help_center/articles/search", Method.Get);
            request.AddQueryParameter("query", context.SearchString);
            articles = (await client.ExecuteWithHandling<SearchResponse<Article>>(request)).Results;
        }

        return articles
            .OrderByDescending(x => x.UpdatedAt)
            .Select(x => new DataSourceItem(x.ContentId.ToString(), x.Title));
    }
}