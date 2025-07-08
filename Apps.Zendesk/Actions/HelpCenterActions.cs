using Apps.Zendesk.Models.Identifiers;
using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.Actions;

[ActionList("General")]
public class HelpCenterActions : BaseInvocable
{
    private ZendeskClient Client { get; }

    public HelpCenterActions(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new ZendeskClient(invocationContext);
    }

    [Action("Search helpcenter locales", Description = "Get all the activated helpcenter locales")]
    public async Task<EnabledLocales> ListLocales()
    {
        var request = new ZendeskRequest($"/api/v2/help_center/locales", Method.Get);
        return await Client.ExecuteWithHandling<EnabledLocales>(request);
    }
}