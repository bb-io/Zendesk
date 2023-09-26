using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class PermissionGroupDataHandler : BaseInvocable, IDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public PermissionGroupDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var client = new ZendeskClient(InvocationContext);
        var request = new ZendeskRequest("/api/v2/guide/permission_groups", Method.Get, Creds);
        var groups = client.GetPaginated<MultiplePermissionGroups>(request).SelectMany(x => x.PermissionGroups);

        return groups
            .OrderBy(x => x.Name)
            .ToDictionary(x => x.Id, x => x.Name);
    }
}