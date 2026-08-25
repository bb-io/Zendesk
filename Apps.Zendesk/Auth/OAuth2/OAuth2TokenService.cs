using Newtonsoft.Json;
using Apps.Zendesk.Constants;
using Apps.Zendesk.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Authentication;
using System.Globalization;

namespace Apps.Zendesk.Auth.OAuth2;

public class OAuth2TokenService(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IOAuth2TokenService, ITokenRefreshable
{
    private const int TokenExpirationBufferMinutes = 5;
    private readonly string _correlationId = Guid.NewGuid().ToString();

    public bool IsRefreshToken(Dictionary<string, string> values)
    {
        // API-token connections and legacy OAuth connections hold no refresh token, so a refresh can only ever fail.
        if (!values.TryGetValue(CredNames.RefreshToken, out var storedRefreshToken) ||
            string.IsNullOrWhiteSpace(storedRefreshToken))
        {
            LogInfo("No refresh token stored for this connection, skipping refresh");
            return false;
        }

        if (!values.TryGetValue(CredNames.ExpiresAt, out var expiresAtString) || string.IsNullOrEmpty(expiresAtString))
        {
            LogInfo("Token expiration info not found, refresh required");
            return true;
        }

        if (!TryParseExpiresAtUtc(expiresAtString, out var expiresAt))
        {
            LogWarning($"Failed to parse expires_at: {expiresAtString}");
            return true;
        }

        var shouldRefresh = DateTime.UtcNow.AddMinutes(TokenExpirationBufferMinutes) > expiresAt;

        if (shouldRefresh)
        {
            var timeUntilExpiration = expiresAt - DateTime.UtcNow;
            LogInfo($"Token expiring soon (at {expiresAt:O}). Time remaining: {timeUntilExpiration:hh\\:mm\\:ss}");
        }

        return shouldRefresh;
    }

    public int? GetRefreshTokenExprireInMinutes(Dictionary<string, string> values)
    {
        if (!values.TryGetValue(CredNames.ExpiresAt, out var expireValue))
            return null;

        if (!TryParseExpiresAtUtc(expireValue, out var expireDate))
            return null;

        var difference = expireDate - DateTime.UtcNow;

        return (int)difference.TotalMinutes - TokenExpirationBufferMinutes;
    }

    public async Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values, CancellationToken cancellationToken) 
    {
        LogInfo("Starting token refresh");

        try
        {
            if (!values.TryGetValue(CredNames.RefreshToken, out var refreshToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new PluginMisconfigurationException(
                    "The Zendesk connection has no refresh token stored, so it cannot be renewed automatically. " +
                    "Please reconnect your Zendesk connection.");
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
            var result = tokenResponse.ToDictionary();

            // Zendesk does not always rotate the refresh token; keep the working one so the connection stays renewable.
            var refreshTokenReturned = result.ContainsKey(CredNames.RefreshToken);
            if (!refreshTokenReturned)
                result[CredNames.RefreshToken] = refreshToken;

            LogInfo($"Token refreshed successfully, expires at: {tokenResponse.ExpiresAt:O}, refresh token rotated: {refreshTokenReturned}");
            return result;
        }
        catch (Exception e)
        {
            LogError($"Failed to refresh token: {e.Message}", e);
            throw;
        }
    }

    public async Task<Dictionary<string, string>> RequestToken(
        string state, 
        string code, 
        Dictionary<string, string> values, 
        CancellationToken cancellationToken)
    {
        LogInfo($"Requesting initial token with state: {state}");

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

    private static bool TryParseExpiresAtUtc(string value, out DateTime expiresAtUtc)
    {
        // expires_at is stored in UTC; without these styles a round-trip ("O") value would be converted to local time.
        const DateTimeStyles styles = DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal;

        return DateTime.TryParse(value, CultureInfo.InvariantCulture, styles, out expiresAtUtc)
               || DateTime.TryParse(value, CultureInfo.CurrentCulture, styles, out expiresAtUtc);
    }

    #region Logging Methods

    private void LogInfo(string message)
    {
        InvocationContext.Logger?.LogInformation(
            $"[ZendeskOAuth2] [{_correlationId}] {message}", []);
    }

    private void LogWarning(string message)
    {
        InvocationContext.Logger?.LogWarning(
            $"[ZendeskOAuth2] [{_correlationId}] {message}", []);
    }

    private void LogError(string message, Exception? exception = null)
    {
        var logMessage = exception != null 
            ? $"[ZendeskOAuth2] [{_correlationId}] {message} | Exception: {exception.GetType().Name}"
            : $"[ZendeskOAuth2] [{_correlationId}] {message}";
        
        InvocationContext.Logger?.LogError(logMessage, []);
    }

    #endregion
}