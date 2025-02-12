using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class PermissionGroupDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public PermissionGroupDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(InvocationContext);
        var request = new ZendeskRequest("/api/v2/guide/permission_groups", Method.Get);
        var groups = (await client.GetPaginated<MultiplePermissionGroups>(request)).SelectMany(x => x.PermissionGroups);

        return groups
            .OrderBy(x => x.Name)
            .ToDictionary(x => x.Id, x => x.Name);
    }
}