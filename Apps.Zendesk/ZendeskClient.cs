using System.Net;
using System.Text;
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
        this.AddDefaultHeader("Authorization", invocationContext.AuthenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
        this.AddDefaultHeader("accept", "*/*");
    }

    private static Uri GetUri(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var url = authenticationCredentialsProviders.First(p => p.KeyName == "api_endpoint").Value;
        return new Uri(url);
    }

    public async Task<List<T>> GetPaginated<T>(ZendeskRequest request) where T : PaginatedResponse
    {
        request.AddQueryParameter("page[size]", 100);
        var results = new List<T>();
        string? next_page;
        do
        {
            var response = await ExecuteWithHandling<T>(request);

            next_page = response.NextPage;
            results.Add(response);
        } while (next_page != null);

        return results;
    }

    public async Task<List<T>> GetPaginatedResults<T>(ZendeskRequest request)
    {
        request.AddQueryParameter("per_page", 100);
        var results = new List<T>();
        string? next_page;
        do
        {
            var response = await ExecuteWithHandling<PaginatedResultResponse<T>>(request);

            next_page = response.Next;
            results.AddRange(response.Results);
        } while (next_page != null);

        return results;
    }

    public async Task<T> ExecuteWithRetries<T>(ZendeskRequest request, int retryCount = 3)
    {
        var response = await ExecuteWithRetries(request, retryCount);
        return JsonConvert.DeserializeObject<T>(response.Content!)!;
    }

    public async Task<RestResponse> ExecuteWithRetries(RestRequest request, int retryCount = 3)
    {
        var attempt = 0;
        var latestErrorMessage = string.Empty;

        while (attempt < retryCount)
        {
            attempt++;
            try
            {
                var response = await ExecuteWithHandling(request);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                latestErrorMessage = response.Content;
            }
            catch (PluginApplicationException ex)
            {
                latestErrorMessage = ex.Message;

                if (attempt >= retryCount)
                    throw;
            }

            await Task.Delay(5000 * attempt);
        }

        throw new PluginApplicationException($"Request failed after {retryCount} attempts. With the latest error: {latestErrorMessage}");
    }

    public async Task<RestResponse> ExecuteWithHandling(RestRequest request)
    {
        RestResponse response;

        try
        {
            response = await ExecuteAsync(request);
        }
        catch (Exception ex)
        {
            throw new PluginApplicationException("Error:", ex);
        }

        var content = response.RawBytes != null
               ? Encoding.UTF8.GetString(response.RawBytes)
               : response.Content ?? string.Empty;

        if (response.ContentType?.Contains("html", StringComparison.OrdinalIgnoreCase) == true
                || content.TrimStart().StartsWith("<"))
        {
            throw new PluginApplicationException($"Expected JSON but received HTML:\n{content}");
        }

        if (response.IsSuccessStatusCode)
            return response;

        var errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content);

        if (errorResponse?.Error == null)
        {
            throw new PluginApplicationException($"Error: {response.Content} ({response.StatusCode})");
        }

        string exceptionMessage;

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            exceptionMessage = errorResponse.Error == "InvalidEndpoint"
                ? "This feature is not allowed for your Zendesk instance"
                : $"Error: {errorResponse.Error}";
        }
        else if (response.StatusCode == HttpStatusCode.Conflict)
        {
            exceptionMessage = errorResponse.Error == "Conflict"
                ? "API response indicates a conflict with the resource you're trying to create or update. This error typically occur when two or more requests try to create or change the same resource simultaneously."
                : $"errorResponse.Error";
        }
        else if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            exceptionMessage = errorResponse.Error == "InternalServerError"
                ? "Error happened on Zenbdesk server. Please check you input and try again"
                : $"Error: {errorResponse.Error}";
        }
        else
        {
            exceptionMessage = $"{errorResponse.Error}: {errorResponse.Description} ({response.StatusCode})";
        }

        throw new PluginApplicationException($"{exceptionMessage}");
    }

    public async Task<T> ExecuteWithHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithHandling(request);
        return JsonConvert.DeserializeObject<T>(response.Content);
    }
}