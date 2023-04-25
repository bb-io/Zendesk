using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Microsoft.AspNetCore.WebUtilities;

namespace Apps.Zendesk.Authorization.OAuth2
{
    public class OAuth2AuthorizeService : IOAuth2AuthorizeService
    {
        public string GetAuthorizationUrl(Dictionary<string, string> values)
        {
            string oauthUrl = $"{values["api_endpoint"].TrimEnd('/')}/oauth/authorizations/new";
            var parameters = new Dictionary<string, string>
            {
                { "client_id", values["client_id"] },
                { "redirect_uri", values["redirect_uri"] },
                { "response_type", "code"},
                { "state", values["state"] },
                { "scope", "read write" }
            };
            return QueryHelpers.AddQueryString(oauthUrl, parameters);
        }
    }
}
