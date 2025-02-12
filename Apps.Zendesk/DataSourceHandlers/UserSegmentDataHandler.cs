using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class UserSegmentDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public UserSegmentDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(InvocationContext);
        var request = new ZendeskRequest("/api/v2/help_center/user_segments/applicable", Method.Get);
        var segments = (await client.GetPaginated<MultipleUserSegments>(request)).SelectMany(x => x.UserSegments);

        return segments
            .OrderBy(x => x.Name)
            .ToDictionary(x => x.Id, x => x.Name);
    }
}