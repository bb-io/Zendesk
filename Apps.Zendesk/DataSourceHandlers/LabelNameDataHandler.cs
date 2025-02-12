using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class LabelNameDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public LabelNameDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        try
        {
            var client = new ZendeskClient(InvocationContext);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/labels", Method.Get);
            var response = await client.ExecuteAsync<LabelResponse>(request);

            return response.Data!.Labels
                .Where(x => context.SearchString == null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(x => x.Name, x => x.Name);
        }
        catch (Exception ex)
        {
            throw new Exception("Article labels are only available on the Professional and Enterprise plans.");
        }
    }
}