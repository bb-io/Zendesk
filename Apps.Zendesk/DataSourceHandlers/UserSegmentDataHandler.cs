using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class UserSegmentDataHandler : BaseInvocable, IDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public UserSegmentDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var client = new ZendeskClient(InvocationContext);
        var request = new ZendeskRequest("/api/v2/help_center/user_segments/applicable", Method.Get, Creds);
        var segments = client.GetPaginated<MultipleUserSegments>(request).SelectMany(x => x.UserSegments);

        return segments
            .OrderBy(x => x.Name)
            .ToDictionary(x => x.Id, x => x.Name);
    }
}