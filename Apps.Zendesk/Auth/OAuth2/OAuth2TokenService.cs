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
    public bool IsRefreshToken(Dictionary<string, string> values) 
    {
        var expiresAt = DateTime.Parse(values[CredNames.ExpiresAt]);
        return DateTime.UtcNow > expiresAt;
    }

    public async Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values, CancellationToken cancellationToken) 
    {
        try
        {
            string tokenUrl = GetTokenUrl(values);

            var parameters = new Dictionary<string, string> 
            {
                { "grant_type", "refresh_token" },
                { "client_id", ApplicationConstants.ClientId },
                { "client_secret", ApplicationConstants.ClientSecret },
                { "refresh_token", values[CredNames.RefreshToken] },
                { "expires_in", "3600" }
            };

            var tokenDto = await ExecuteTokenRequest(parameters, tokenUrl, cancellationToken);
            var dictionary = TokenDtoToDictionary(tokenDto);
            AddExpiresAt(dictionary);
            return dictionary;
        }
        catch (Exception e)
        {
            var valuesString = string.Join(", ", values.Select(kv => $"{kv.Key}: {kv.Value}"));
            invocationContext.Logger?.LogError($"[ZendeskOAuth2TokenService] Error ({e.GetType()}) refreshing token: {e.Message}. Values: {valuesString}", []);
            throw;
        }
    }

    public async Task<Dictionary<string, string?>> RequestToken(
        string state, 
        string code, 
        Dictionary<string, string> values, 
        CancellationToken cancellationToken)
    {
        string tokenUrl = GetTokenUrl(values);
        string redirectUri = $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode";

        var bodyParameters = new Dictionary<string, string> 
        {
            { "grant_type", "authorization_code" },
            { "client_id", ApplicationConstants.ClientId },
            { "client_secret", ApplicationConstants.ClientSecret },
            { "redirect_uri", redirectUri },
            { "scope", ApplicationConstants.Scope },
            { "code", code },
            { "expires_in", "3600" }
        };

        var tokenDto = await ExecuteTokenRequest(bodyParameters, tokenUrl, cancellationToken);
        var dictionary = TokenDtoToDictionary(tokenDto);
        AddExpiresAt(dictionary);
        return dictionary;
    }

    public Task RevokeToken(Dictionary<string, string> values) 
    {
        throw new NotImplementedException();
    }

    private async Task<TokenDto> ExecuteTokenRequest(Dictionary<string, string> parameters,
        string tokenUrl,
        CancellationToken cancellationToken) 
    {
        using var client = new HttpClient();
        using var content = new FormUrlEncodedContent(parameters);
        using var response = await client.PostAsync(tokenUrl, content, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode) 
        {
            throw new Exception($"Error requesting token: {response.StatusCode} - {responseContent}");
        }

        return JsonConvert.DeserializeObject<TokenDto>(responseContent!)!;
    }

    private Dictionary<string, string> AddExpiresAt(Dictionary<string, string> dictionary) 
    {
        if (!dictionary.TryGetValue(CredNames.ExpiresIn, out var expiresAtSeconds) || string.IsNullOrEmpty(expiresAtSeconds))
        {
            expiresAtSeconds = "3600";
        }

        var expiresAt = DateTime.UtcNow.AddSeconds(int.Parse(expiresAtSeconds));
        dictionary[CredNames.ExpiresAt] = expiresAt.ToString();
        return dictionary;
    }

    private string GetTokenUrl(Dictionary<string, string> values) 
    {
        if (!values.TryGetValue("api_endpoint", out var endpoint) || string.IsNullOrEmpty(endpoint)) 
        {
            throw new KeyNotFoundException("api_endpoint not found or empty");
        }

        var uri = new Uri(endpoint);
        var baseUrl = uri.GetLeftPart(UriPartial.Authority).TrimEnd('/');
        return $"{baseUrl}/oauth/tokens";
    }

    private Dictionary<string, string?> TokenDtoToDictionary(TokenDto tokenDto) 
    {
        var result = new Dictionary<string, string?> 
        {
            { "access_token", tokenDto.AccessToken },
            { "refresh_token", tokenDto.RefreshToken },
            { "token_type", tokenDto.TokenType },
            { "scope", tokenDto.Scope },
            { "refresh_token_expires_in", tokenDto.RefreshTokenExpiresIn?.ToString() }
        };

        if (tokenDto.AdditionalData != null) 
        {
            foreach (var kv in tokenDto.AdditionalData) 
            {
                result[kv.Key] = kv.Value.ToString();
            }
        }

        return result;
    }
}