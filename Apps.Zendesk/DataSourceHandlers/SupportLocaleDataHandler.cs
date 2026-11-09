using Apps.Zendesk.Models.DynamicContent;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class SupportLocaleDataHandler(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(
        DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(InvocationContext);
        var response = await client.ExecuteWithHandling<SupportLocalesResponse>(
            new ZendeskRequest("/api/v2/locales", Method.Get));

        return response.Locales
            .Where(x => string.IsNullOrWhiteSpace(context.SearchString)
                || x.LocaleCode.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase)
                || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.LocaleCode, x.Name));
    }
}
