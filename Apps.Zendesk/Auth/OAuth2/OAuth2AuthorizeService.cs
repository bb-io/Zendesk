﻿using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
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
                { "client_id", ApplicationConstants.ClientId},
                { "redirect_uri", ApplicationConstants.RedirectUri },
                { "response_type", "code"},
                { "state", values["state"] },
                { "scope", ApplicationConstants.Scope }
            };
            return QueryHelpers.AddQueryString(oauthUrl, parameters);
        }
    }
}
