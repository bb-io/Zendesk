using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class LocaleDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public LocaleDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(InvocationContext);
        var request = new ZendeskRequest($"/api/v2/help_center/locales", Method.Get);
        var response = await client.ExecuteWithHandling<EnabledLocales>(request);

        return response.Locales
            .Where(x => context.SearchString == null ||
                        x.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .ToDictionary(x => x, x => x);
    }
}
