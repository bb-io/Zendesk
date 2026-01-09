using Newtonsoft.Json;
using Apps.Zendesk.Constants;
using Apps.Zendesk.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Auth.OAuth2;

public class OAuth2TokenService(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IOAuth2TokenService
{
    private const int TokenExpirationBufferMinutes = 5;
    private readonly string _correlationId = Guid.NewGuid().ToString();

    public bool IsRefreshToken(Dictionary<string, string> values)
    {
        WebhookLogger.Log("IsRefreshToken start");
        WebhookLogger.Log(values);
        if (!values.TryGetValue(CredNames.ExpiresAt, out var expiresAtString) || string.IsNullOrEmpty(expiresAtString))
        {
            LogInfo("Token expiration info not found, refresh required");
            return true;
        }

        if (!DateTime.TryParse(expiresAtString, out var expiresAt))
        {
            LogWarning($"Failed to parse expires_at: {expiresAtString}");
            return true;
        }

        var shouldRefresh = DateTime.UtcNow.AddMinutes(TokenExpirationBufferMinutes) > expiresAt;
        var timeUntilExpiration = expiresAt - DateTime.UtcNow;
        LogInfo($"Token expires at {expiresAt:O}, time until expiration: {timeUntilExpiration:hh\\:mm\\:ss}, refresh required: {shouldRefresh}");
        
        return shouldRefresh;
    }

    public async Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values, CancellationToken cancellationToken) 
    {
        LogInfo("Starting token refresh");
        WebhookLogger.Log(values);

        try
        {
            if (!values.TryGetValue(CredNames.RefreshToken, out var refreshToken) || string.IsNullOrEmpty(refreshToken))
            {
                throw new InvalidOperationException("Refresh token not found in credentials");
            }

            var tokenUrl = GetTokenUrl(values);
            LogInfo($"Token URL: {tokenUrl}");

            var request = new OAuth2TokenRequest
            {
                GrantType = "refresh_token",
                ClientId = ApplicationConstants.ClientId,
                ClientSecret = ApplicationConstants.ClientSecret,
                RefreshToken = refreshToken
            };

            var tokenResponse = await ExecuteTokenRequestAsync(request, tokenUrl, cancellationToken);
            LogInfo($"Token refreshed successfully, expires at: {tokenResponse.ExpiresAt:O}");
            WebhookLogger.Log("[RefreshToken] Token response");
            WebhookLogger.Log(tokenResponse);

            WebhookLogger.Log("tokenResponse dictionary"); 
            WebhookLogger.Log(tokenResponse.ToDictionary());
            return tokenResponse.ToDictionary();
        }
        catch (Exception e)
        {
            LogError($"Failed to refresh token: {e.Message}", e);
            throw;
        }
    }

    public async Task<Dictionary<string, string?>> RequestToken(
        string state, 
        string code, 
        Dictionary<string, string> values, 
        CancellationToken cancellationToken)
    {
        LogInfo($"Requesting initial token with state: {state}");
        WebhookLogger.Log(values);

        try
        {
            var tokenUrl = GetTokenUrl(values);
            var redirectUri = $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode";
            
            LogInfo($"Token URL: {tokenUrl}, Redirect URI: {redirectUri}");
            var request = new OAuth2TokenRequest
            {
                GrantType = "authorization_code",
                ClientId = ApplicationConstants.ClientId,
                ClientSecret = ApplicationConstants.ClientSecret,
                RedirectUri = redirectUri,
                Scope = ApplicationConstants.Scope,
                Code = code
            };

            var tokenResponse = await ExecuteTokenRequestAsync(request, tokenUrl, cancellationToken);
            LogInfo($"Token requested successfully, expires at: {tokenResponse.ExpiresAt:O}");
            WebhookLogger.Log(tokenResponse);
            WebhookLogger.Log(tokenResponse.ToDictionary());
            return tokenResponse.ToDictionary();
        }
        catch (Exception e)
        {
            LogError($"Failed to request token: {e.Message}", e);
            throw;
        }
    }

    public Task RevokeToken(Dictionary<string, string> values) 
    {
        throw new NotImplementedException();
    }

    private async Task<OAuth2TokenResponse> ExecuteTokenRequestAsync(
        OAuth2TokenRequest request,
        string tokenUrl,
        CancellationToken cancellationToken) 
    {
        LogInfo($"Executing token request to {tokenUrl}");

        using var client = new HttpClient();
        using var content = new FormUrlEncodedContent(request.ToFormData());
        
        var response = await client.PostAsync(tokenUrl, content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode) 
        {
            LogError($"Token request failed with status {response.StatusCode}: {responseContent}");
            throw new HttpRequestException($"Token request failed: {response.StatusCode} - {responseContent}");
        }

        LogInfo("Token request successful, deserializing response");
        
        var tokenDto = JsonConvert.DeserializeObject<TokenDto>(responseContent)
            ?? throw new InvalidOperationException("Failed to deserialize token response");
        WebhookLogger.Log(responseContent);

        return OAuth2TokenResponse.FromTokenDto(tokenDto);
    }

    private string GetTokenUrl(Dictionary<string, string> values) 
    {
        if (!values.TryGetValue(CredNames.BaseUrl, out var endpoint) || string.IsNullOrEmpty(endpoint)) 
        {
            throw new InvalidOperationException("API endpoint not found in connection values");
        }

        var uri = new Uri(endpoint);
        var baseUrl = uri.GetLeftPart(UriPartial.Authority).TrimEnd('/');
        return $"{baseUrl}/oauth/tokens";
    }

    #region Logging Methods

    private void LogInfo(string message)
    {
        InvocationContext.Logger?.LogInformation(
            $"[ZendeskOAuth2] [{_correlationId}] {message}", []);
        WebhookLogger.Log(message);
    }

    private void LogWarning(string message)
    {
        InvocationContext.Logger?.LogWarning(
            $"[ZendeskOAuth2] [{_correlationId}] {message}", []);
        WebhookLogger.Log(message);
    }

    private void LogError(string message, Exception? exception = null)
    {
        var logMessage = exception != null 
            ? $"[ZendeskOAuth2] [{_correlationId}] {message} | Exception: {exception.GetType().Name}"
            : $"[ZendeskOAuth2] [{_correlationId}] {message}";
        
        InvocationContext.Logger?.LogError(logMessage, []);
        WebhookLogger.Log(logMessage);
    }

    #endregion
}