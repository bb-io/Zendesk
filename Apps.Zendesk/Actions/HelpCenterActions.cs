using Apps.Zendesk.Models.Identifiers;
using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.Actions
{
    [ActionList]
    public class HelpCenterActions : BaseInvocable
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        public HelpCenterActions(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        [Action("Get helpcenter locales", Description = "Get all the activated helpcenter locales")]
        public EnabledLocales ListLocales()
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/locales", Method.Get, Creds);
            return client.Get<EnabledLocales>(request);
        }

        [Action("Delete translation", Description = "Delete a specific translation")]
        public async Task DeleteTranslation([ActionParameter] TranslationIdentifier translation)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/translations/{translation.Id}", Method.Delete, Creds);
            await client.ExecuteWithHandling(request);
        }
    }
}