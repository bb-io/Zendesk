using Apps.Zendesk.Dtos;
using Apps.Zendesk.Models.Responses.Error;
using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Zendesk
{
    public class ZendeskClient : RestClient
    {
        public ZendeskClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) :
            base(new RestClientOptions()
                { ThrowOnAnyError = false, BaseUrl = GetUri(authenticationCredentialsProviders) })
        {
        }

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
            => ExecuteWithHandling<T>(request).Result;

        public async Task<RestResponse> ExecuteWithHandling(ZendeskRequest request)
        {
            var response = await ExecuteAsync(request);

            if (response.IsSuccessStatusCode)
                return response;

            var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content)!;
            throw new($"{error.Error.Title}: {error.Error.Message}");
        }

        public async Task<T> ExecuteWithHandling<T>(ZendeskRequest request)
        {
            var response = await ExecuteWithHandling(request);
            return JsonConvert.DeserializeObject<T>(response.Content);
        }
    }
}