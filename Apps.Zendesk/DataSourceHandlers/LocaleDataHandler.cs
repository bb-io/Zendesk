using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class LocaleDataHandler : BaseInvocable, IAsyncDataSourceItemHandler
{
    public LocaleDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(InvocationContext);
        var request = new ZendeskRequest($"/api/v2/help_center/locales", Method.Get);
        var response = await client.ExecuteWithHandling<EnabledLocales>(request);

        var allLocalesRequest = new ZendeskRequest($"/api/v2/locales/public", Method.Get);
        var allLocalesResponse = await client.ExecuteWithHandling<MultipleLocales>(allLocalesRequest);

        return response.Locales
            .Where(x => context.SearchString == null ||
                        x.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x, allLocalesResponse.Locales.FirstOrDefault(y => y.LocaleCode.Equals(x, StringComparison.OrdinalIgnoreCase))?.PresentationName ?? x));
    }
}
