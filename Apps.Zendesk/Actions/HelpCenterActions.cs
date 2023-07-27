using Apps.OpenAI.Models.Responses;
using Apps.Zendesk.Dtos;
using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Actions
{
    [ActionList]
    public class HelpCenterActions
    {
        [Action("Get helpcenter locales", Description = "Get all the activated helpcenter locales")]
        public AllLocalesResponse ListArticles(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var client = new ZendeskClient(authenticationCredentialsProviders);
            var request = new ZendeskRequest($"/api/v2/help_center/locales",
                Method.Get, authenticationCredentialsProviders);
            return client.Get<AllLocalesResponse>(request);
        }
    }
}
