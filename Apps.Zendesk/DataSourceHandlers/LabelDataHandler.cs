using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class LabelDataHandler(InvocationContext invocationContext) : BaseInvocable(invocationContext), IAsyncDataSourceHandler
{
    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        try
        {
            var client = new ZendeskClient(InvocationContext);
            var request = new ZendeskRequest($"/api/v2/help_center/articles/labels", Method.Get);
            var response = await client.ExecuteWithHandling<LabelResponse>(request);

            return response.Labels
                .Where(x => context.SearchString == null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(x => x.Id.ToString(), x => x.Name);
        } catch (Exception ex)
        {
            throw new Exception("Article labels are only available on the Professional and Enterprise plans.");
        }

    }
}