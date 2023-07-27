using Apps.OpenAI.Models.Responses;
using Apps.Zendesk.Dtos;
using Apps.Zendesk.Models.Responses;
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

        public List<T> GetPaginated<T>(ZendeskRequest request) where T : PaginatedResponse
        {
            var results = new List<T>();
            string? next_page;
            do
            {
                var result = this.Get<T>(request);
                next_page = result.NextPage;
                results.Add(result);
            } while (next_page != null);

            return results;
        }

    }
}
