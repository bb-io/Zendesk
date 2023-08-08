using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Zendesk.Actions
{
    [ActionList]
    public class HelpCenterActions
    {
        [Action("Get helpcenter locales", Description = "Get all the activated helpcenter locales")]
        public AllLocalesResponse ListLocales(
            IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/locales",
                Method.Get, authenticationCredentialsProviders);
            return client.Get<AllLocalesResponse>(request);
        }
    }
}