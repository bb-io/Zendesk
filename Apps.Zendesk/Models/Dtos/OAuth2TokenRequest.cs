namespace Apps.Zendesk.Models.Dtos;

internal class OAuth2TokenRequest
{
    public string GrantType { get; init; } = null!;
    public string ClientId { get; init; } = null!;
    public string ClientSecret { get; init; } = null!;
    public string? RefreshToken { get; init; }
    public string? Code { get; init; }
    public string? RedirectUri { get; init; }
    public string? Scope { get; init; }

    public Dictionary<string, string> ToFormData()
    {
        var data = new Dictionary<string, string>
        {
            { "grant_type", GrantType },
            { "client_id", ClientId },
            { "client_secret", ClientSecret }
        };

        if (!string.IsNullOrEmpty(RefreshToken))
            data["refresh_token"] = RefreshToken;

        if (!string.IsNullOrEmpty(Code))
            data["code"] = Code;

        if (!string.IsNullOrEmpty(RedirectUri))
            data["redirect_uri"] = RedirectUri;

        if (!string.IsNullOrEmpty(Scope))
            data["scope"] = Scope;

        return data;
    }
}
