using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk
{
    public class ZendeskClient : RestClient
    {
        public ZendeskClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : 
            base(new RestClientOptions() { ThrowOnAnyError = false, BaseUrl = GetUri(authenticationCredentialsProviders) }) { }

        private static Uri GetUri(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        {
            var url = authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value;
            return new Uri(url);
        }
    }
}
