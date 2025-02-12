using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class AdminDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public AdminDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(InvocationContext);
        var request = new ZendeskRequest("/api/v2/users", Method.Get);
        request.AddQueryParameter("role", "admin");
        var users = (await client.GetPaginated<MultipleUsers>(request)).SelectMany(x => x.Users);

        return users
            .OrderBy(x => x.Name)
            .ToDictionary(x => x.Id, x => x.Name);
    }
}