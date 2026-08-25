using Apps.Zendesk.Constants;

namespace Apps.Zendesk.Models.Dtos;

public class OAuth2TokenResponse
{
    public string AccessToken { get; init; } = null!;
    public string? RefreshToken { get; init; }
    public string? TokenType { get; init; }
    public string? Scope { get; init; }
    public int? ExpiresIn { get; init; }
    public DateTime? ExpiresAt { get; init; }
    public int? RefreshTokenExpiresIn { get; init; }

    public Dictionary<string, string?> ToDictionary()
    {
        var dict = new Dictionary<string, string?>
        {
            { "access_token", AccessToken },
            { "token_type", TokenType },
            { "scope", Scope },
            { "expires_in", ExpiresIn.ToString() },
            { "refresh_token_expires_in", RefreshTokenExpiresIn?.ToString() }
        };

        // Never emit a null refresh token: it would overwrite the stored one and leave the connection unrenewable.
        if (!string.IsNullOrWhiteSpace(RefreshToken))
            dict[CredNames.RefreshToken] = RefreshToken;

        if (ExpiresAt.HasValue)
            dict[CredNames.ExpiresAt] = ExpiresAt.Value.ToString("O");

        return dict;
    }

    public static OAuth2TokenResponse FromTokenDto(TokenDto tokenDto)
    {
        // Zendesk omits both fields for non-expiring tokens, in which case there is no expiry to store.
        DateTime? expiresAt = null;
        if (tokenDto.ExpiresIn.HasValue)
            expiresAt = DateTime.UtcNow.AddSeconds(tokenDto.ExpiresIn.Value);
        else if (tokenDto.RefreshTokenExpiresIn.HasValue)
            expiresAt = DateTime.UtcNow.AddSeconds(tokenDto.RefreshTokenExpiresIn.Value);

        return new OAuth2TokenResponse
        {
            AccessToken = tokenDto.AccessToken,
            RefreshToken = tokenDto.RefreshToken,
            TokenType = tokenDto.TokenType,
            Scope = tokenDto.Scope,
            ExpiresIn = tokenDto.ExpiresIn,
            ExpiresAt = expiresAt,
            RefreshTokenExpiresIn = tokenDto.RefreshTokenExpiresIn
        };
    }
}
