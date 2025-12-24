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
    private const int DefaultMaxRetries = 5;
    private const int InternalServerErrorBaseDelayMs = 3000;
    private const int InternalServerErrorMaxDelayMs = 15000;
    private const int RateLimitMaxDelayMs = 60000;
    private const int RateLimitJitterMaxMs = 1000;

    private InvocationContext Context { get; }

    public string BaseUrl { get; set; }

    public ZendeskClient(InvocationContext invocationContext) :
        base(new RestClientOptions()
        { ThrowOnAnyError = false, BaseUrl = GetUri(invocationContext.AuthenticationCredentialsProviders), MaxTimeout = 600000 })
    {
        Context = invocationContext;
        
        var connectionType = invocationContext.AuthenticationCredentialsProviders
            .FirstOrDefault(p => p.KeyName == CredNames.ConnectionType)?.Value;
        
        string authorizationHeader;
        if (connectionType == ConnectionTypes.ApiToken)
        {
            var email = invocationContext.AuthenticationCredentialsProviders
                .FirstOrDefault(p => p.KeyName == CredNames.Email)?.Value;
            var apiToken = invocationContext.AuthenticationCredentialsProviders
                .FirstOrDefault(p => p.KeyName == CredNames.AccessToken)?.Value;
            
            var credentials = $"{email}/token:{apiToken}";
            var encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes(credentials));
            authorizationHeader = $"Basic {encodedCredentials}";
        }
        else
        {
            var accessToken = invocationContext.AuthenticationCredentialsProviders
                .FirstOrDefault(p => p.KeyName == CredNames.AccessToken)?.Value;
            authorizationHeader = $"Bearer {accessToken}";
        }
        
        this.AddDefaultHeader("Authorization", authorizationHeader);
        this.AddDefaultHeader("accept", "*/*");
        BaseUrl = GetUri(invocationContext.AuthenticationCredentialsProviders).ToString().TrimEnd('/');
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

    public async Task<RestResponse> ExecuteWithHandling(RestRequest request, int maxRetries = DefaultMaxRetries)
    {
        int attempt = 0;
        RestResponse response;
        string latestErrorMessage = string.Empty;

        while (attempt <= maxRetries)
        {
            try
            {
                response = await ExecuteAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return response;
                }

                if (ShouldRetry(response.StatusCode))
                {
                    attempt++;

                    if (attempt > maxRetries)
                    {
                        Context.Logger?.LogWarning($"[ZendeskClient] Maximum retry attempts reached ({maxRetries}) for {response.StatusCode}", []);
                        break;
                    }

                    Context.Logger?.LogWarning($"[ZendeskClient] Retrying request (attempt {attempt}/{maxRetries}) due to {response.StatusCode}", []);
                    latestErrorMessage = $"Status code: {response.StatusCode}, Content: {response.Content}";

                    TimeSpan delay;
                    if (response.StatusCode == HttpStatusCode.TooManyRequests)
                    {
                        delay = CalculateDelayForRateLimit(response, attempt);
                    }
                    else // InternalServerError
                    {
                        delay = TimeSpan.FromMilliseconds(Math.Min(InternalServerErrorBaseDelayMs * attempt, InternalServerErrorMaxDelayMs));
                    }

                    await Task.Delay(delay);
                    continue;
                }

                return ProcessErrorResponse(response);
            }
            catch (Exception ex) when (!(ex is PluginApplicationException))
            {
                latestErrorMessage = ex.Message;
                Context.Logger?.LogError($"[ZendeskClient] Unexpected error: {ex.Message}", []);
                throw new PluginApplicationException("Error during request execution", ex);
            }
        }

        throw new PluginApplicationException($"Request failed after {maxRetries} attempts. Latest error: {latestErrorMessage}");
    }

    public async Task<T> ExecuteWithHandling<T>(RestRequest request)
    {
        var response = await ExecuteWithHandling(request);

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

    private TimeSpan CalculateDelayForRateLimit(RestResponse response, int attempt)
    {
        if (response.Headers != null &&
            response.Headers.Any(h => h.Name?.Equals("Retry-After", StringComparison.OrdinalIgnoreCase) == true))
        {
            var retryHeader = response.Headers.First(h =>
                h.Name?.Equals("Retry-After", StringComparison.OrdinalIgnoreCase) == true).Value?.ToString();

            if (int.TryParse(retryHeader, out int delaySeconds))
            {
                Context.Logger?.LogInformation($"[ZendeskClient] Using Retry-After header: {delaySeconds} seconds", []);
                return TimeSpan.FromSeconds(delaySeconds);
            }
        }

        var baseDelay = Math.Pow(2, attempt) * 1000;
        var jitter = new Random().Next(0, RateLimitJitterMaxMs);
        var delay = Math.Min(baseDelay + jitter, RateLimitMaxDelayMs);

        Context.Logger?.LogInformation($"[ZendeskClient] Using exponential backoff: {delay}ms", []);
        return TimeSpan.FromMilliseconds(delay);
    }

    private bool ShouldRetry(HttpStatusCode statusCode)
    {
        return statusCode == HttpStatusCode.TooManyRequests ||
               statusCode == HttpStatusCode.InternalServerError;
    }

    private RestResponse ProcessErrorResponse(RestResponse response)
    {
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
                ? "API response indicates a conflict with the resource you're trying to create or update. This error typically occurs when two or more requests try to create or change the same resource simultaneously."
                : $"{errorResponse.Error}";
        }
        else if (response.StatusCode == HttpStatusCode.InternalServerError)
        {
            exceptionMessage = errorResponse.Error == "InternalServerError"
                ? "Error happened on Zendesk server. Retrying the request."
                : $"Error: {errorResponse.Error}";

            // For InternalServerError, we'll let the retry mechanism handle it
            return response;
        }
        else if (response.StatusCode == HttpStatusCode.TooManyRequests)
        {
            // For TooManyRequests, we'll let the retry mechanism handle it
            return response;
        }
        else
        {
            exceptionMessage = $"{errorResponse.Error}: {errorResponse.Description} ({response.StatusCode})";
        }

        throw new PluginApplicationException($"{exceptionMessage}");
    }
}