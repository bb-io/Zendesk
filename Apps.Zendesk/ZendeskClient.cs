using System.Net;
using System.Text;
using Apps.Zendesk.Constants;
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
        this.AddDefaultHeader("Authorization", invocationContext.AuthenticationCredentialsProviders.First(p => p.KeyName == CredNames.AccessToken).Value);
        this.AddDefaultHeader("accept", "*/*");
    }

    private static Uri GetUri(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var url = authenticationCredentialsProviders.First(p => p.KeyName == CredNames.BaseUrl).Value;
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
        const int pageSize = 100;
        request.AddQueryParameter("per_page", pageSize);

        var results = new List<T>();
        string? nextPage = null;
        long totalItems = 0;
        bool firstRequest = true;

        do
        {
            if (!firstRequest && nextPage != null)
            {
                request = new ZendeskRequest(nextPage, Method.Get);
            }

            var response = await ExecuteWithHandling<PaginatedResultResponse<T>>(request);

            if (firstRequest)
            {
                totalItems = response.TotalCount;
                firstRequest = false;
            }

            results.AddRange(response.Results);
            nextPage = response.Next;

            if (results.Count >= totalItems || response.Results.Count == 0)
            {
                break;
            }
        } while (nextPage != null);

        return results;
    }

    public async Task<T> ExecuteWithRetries<T>(ZendeskRequest request, int retryCount = 3)
    {
        var response = await ExecuteWithRetries(request, retryCount);

        try
        {
            return JsonConvert.DeserializeObject<T>(response.Content!)!;
        }
        catch (JsonException ex)
        {
            Context.Logger?.LogError($"[ZendeskClient] Failed to deserialize response: {ex.Message}. Response content: {response.Content}", []);
            throw new PluginApplicationException($"Failed to deserialize response: {ex.Message}. Ask blackbird support to check the response content: {response.Content}", ex);
        }
    }

    public async Task<RestResponse> ExecuteWithRetries(RestRequest request, int retryCount = 6)
    {
        var attempt = 0;
        var latestErrorMessage = string.Empty;

        while (attempt < retryCount)
        {
            attempt++;
            try
            {
                var response = await ExecuteAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                
                if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    Context.Logger?.LogWarning($"[ZendeskClient] Rate limit exceeded (attempt {attempt}/{retryCount}). StatusCode: {response.StatusCode}", []);
                    
                    int delaySeconds = 0;
                    if (response.Headers != null && 
                        response.Headers.Any(h => h.Name?.Equals("Retry-After", StringComparison.OrdinalIgnoreCase) == true) &&
                        int.TryParse(response.Headers.First(h => 
                            h.Name?.Equals("Retry-After", StringComparison.OrdinalIgnoreCase) == true).Value?.ToString(), 
                            out delaySeconds))
                    {
                        await Task.Delay(delaySeconds * 1000);
                    }
                    else
                    {
                        var baseDelay = Math.Pow(2, attempt) * 1000;
                        var jitter = new Random().Next(0, 1000);
                        var delay = Math.Min(baseDelay + jitter, 60000);
                        
                        Context.Logger?.LogInformation($"[ZendeskClient] Using exponential backoff: {delay}ms", []);
                        await Task.Delay((int)delay);
                    }
                    
                    latestErrorMessage = $"Rate limit exceeded: {response.Content}";
                    continue;
                }
                
                try
                {
                    return await ExecuteWithHandling(request);
                }
                catch (PluginApplicationException ex)
                {
                    latestErrorMessage = ex.Message;
                    
                    if (attempt >= retryCount)
                        throw;
                    
                    await Task.Delay(5000 * attempt);
                }
            }
            catch (Exception ex) when (!(ex is PluginApplicationException))
            {
                latestErrorMessage = ex.Message;
                Context.Logger?.LogError($"[ZendeskClient] Unexpected error during retry {attempt}/{retryCount}: {ex.Message}", []);

                if (attempt >= retryCount)
                {
                    throw new PluginApplicationException("Error during request execution after multiple retries", ex);
                }
                    
                await Task.Delay(5000 * attempt);
            }
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
        {
            return response;
        }

        ErrorResponse? errorResponse = null;

        try
        {
            errorResponse = JsonConvert.DeserializeObject<ErrorResponse>(response.Content!);
        }
        catch (JsonException)
        {
            Context.Logger?.LogError($"[ZendeskClient] Failed to parse error response ({response.StatusCode}): {response.Content}", []);
            throw new PluginApplicationException($"Failed to parse error response ({response.StatusCode}): {response.Content}");
        }

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
                ? "Error happened on Zenbdesk server. Please, add retry policy to this action."
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