using Apps.Zendesk.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using RestSharp;

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
                var response = Execute<T>(request);
                
                next_page = response.NextPage;
                results.Add(response);
            } while (next_page != null);

            return results;
        }
        
        public T Execute<T>(ZendeskRequest request)
        {
            var response = this.Execute(request);

            if (!response.IsSuccessStatusCode)
                throw new(response.Content);

            return JsonConvert.DeserializeObject<T>(response.Content);
        }

    }
}
