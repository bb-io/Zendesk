using Newtonsoft.Json;
using Apps.Zendesk.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Auth.OAuth2;

public class OAuth2TokenService : BaseInvocable, IOAuth2TokenService {
    private string TokenUrl = "";

    public OAuth2TokenService(InvocationContext invocationContext) : base(invocationContext) { }

    public bool IsRefreshToken(Dictionary<string, string> values) {
        var expiresAt = DateTime.Parse(values[CredNames.ExpiresAt]);
        return DateTime.UtcNow > expiresAt;
    }

    public async Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values, CancellationToken cancellationToken) {
        TokenUrl = $"{new Uri(values["api_endpoint"]).GetLeftPart(UriPartial.Authority).TrimEnd('/')}/oauth/tokens";
        
        var parameters = new Dictionary<string, string> {
            { "grant_type", "refresh_token" },
            { "client_id", ApplicationConstants.ClientId },
            { "client_secret", ApplicationConstants.ClientSecret },
            { "refresh_token", values[CredNames.RefreshToken] },
            { "expires_in", "3600" }
        };

        var dictionary = await ExecuteTokenRequest(parameters, cancellationToken);
        AddExpiresAt(dictionary);
        return dictionary;
    }

    public async Task<Dictionary<string, string?>> RequestToken(
        string state, 
        string code, 
        Dictionary<string, string> values, 
        CancellationToken cancellationToken)
    {
        TokenUrl = $"{new Uri(values["api_endpoint"]).GetLeftPart(UriPartial.Authority).TrimEnd('/')}/oauth/tokens";
        string redirectUri = $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode";

        var bodyParameters = new Dictionary<string, string> {
            { "grant_type", "authorization_code" },
            { "client_id", ApplicationConstants.ClientId },
            { "client_secret", ApplicationConstants.ClientSecret },
            { "redirect_uri", redirectUri },
            { "scope", ApplicationConstants.Scope },
            { "code", code }
        };

        var dictionary = await ExecuteTokenRequest(bodyParameters, cancellationToken);
        AddExpiresAt(dictionary);
        return dictionary;
    }

    public Task RevokeToken(Dictionary<string, string> values) {
        throw new NotImplementedException();
    }

    private async Task<Dictionary<string, string>> ExecuteTokenRequest(Dictionary<string, string> parameters,
        CancellationToken cancellationToken) {
        using var client = new HttpClient();
        using var content = new FormUrlEncodedContent(parameters);
        using var response = await client.PostAsync(TokenUrl, content, cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        if (!response.IsSuccessStatusCode) {
            throw new Exception($"Error requesting token: {response.StatusCode} - {responseContent}");
        }

        return JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent!)!;
    }

    private Dictionary<string, string> AddExpiresAt(Dictionary<string, string> dictionary) {
        var expiresAt = DateTime.UtcNow.AddSeconds(int.Parse(dictionary[CredNames.ExpiresIn]));
        dictionary.Add(CredNames.ExpiresAt, expiresAt.ToString());
        return dictionary;
    }
}