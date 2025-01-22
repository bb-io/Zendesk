using System.Net;
using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Models.Responses.Error;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Zendesk;

public class ZendeskClient : RestClient
{
    private InvocationContext Context { get; }
    public ZendeskClient(InvocationContext invocationContext) :
        base(new RestClientOptions()
            { ThrowOnAnyError = false, BaseUrl = GetUri(invocationContext.AuthenticationCredentialsProviders) })
    {
        Context = invocationContext;
    }

    private static Uri GetUri(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var url = authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value;
        return new Uri(url);
    }

    public List<T> GetPaginated<T>(ZendeskRequest request) where T : PaginatedResponse
    {
        request.AddQueryParameter("page[size]", 100);
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
        => ExecuteWithHandling<T>(request).GetAwaiter().GetResult();

    public async Task<RestResponse> ExecuteWithHandling(RestRequest request)
    {
        try
        {
            //Context.Logger.LogInformation("zendesk-request", new object[] { request });

            var response = await ExecuteAsync(request);

            //Context.Logger.LogInformation("zendesk-response", new object[] { response });

            if (response.IsSuccessStatusCode)
                return response;

            var responseContent = response.Content!;

            var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);

            if (errorResponse?.Error == null)
            {
                throw new PluginApplicationException($"Error: {responseContent} ({response.StatusCode})");
            }

            string exceptionMessage;

            if (response.StatusCode is HttpStatusCode.NotFound)
            {
                exceptionMessage = errorResponse.Error.Title == "InvalidEndpoint"
                    ? "Feature is not allowed for your Zendesk instance"
                    : errorResponse.Error.Message;
            }
            else if (response.StatusCode is HttpStatusCode.Conflict)
            {
                exceptionMessage = errorResponse.Error.Title == "Conflict"
                    ? "Authentication failed due to a conflict with an existing user ID"
                    : errorResponse.Error.Message;
            }
            else
            {
                exceptionMessage = $"{errorResponse.Error.Title}: {errorResponse.Error.Message} ({response.StatusCode})";
            }

            throw new PluginApplicationException(exceptionMessage);
        }
        catch (Exception ex)
        {
            throw new PluginApplicationException("Error:", ex);
        }
    }

    public async Task<T> ExecuteWithHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithHandling(request);
        return JsonConvert.DeserializeObject<T>(response.Content);
    }
}