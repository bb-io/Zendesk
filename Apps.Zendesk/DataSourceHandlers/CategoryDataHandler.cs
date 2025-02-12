using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using Apps.Zendesk.Models.Responses.Wrappers;

namespace Apps.Zendesk.DataSourceHandlers;

public class CategoryDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public CategoryDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(InvocationContext);
        var request = new ZendeskRequest("/api/v2/help_center/categories", Method.Get);
        var categories = (await client.GetPaginated<MultipleCategories>(request)).SelectMany(x => x.Categories);

        return categories
            .OrderByDescending(x => x.UpdatedAt)
            .ToDictionary(x => x.Id.ToString(), x => x.Name);
    }
}