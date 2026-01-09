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
            { "refresh_token", RefreshToken },
            { "token_type", TokenType },
            { "scope", Scope },
            { "expires_in", ExpiresIn.ToString() },
            { "refresh_token_expires_in", RefreshTokenExpiresIn?.ToString() }
        };
        if (ExpiresAt.HasValue)
            dict[CredNames.ExpiresAt] = ExpiresAt.Value.ToString("O");

        return dict;
    }

    public static OAuth2TokenResponse FromTokenDto(TokenDto tokenDto)
    {
        DateTime? expiresAt;
        if (tokenDto.ExpiresIn == null)
            expiresAt = DateTime.UtcNow.AddSeconds(tokenDto.RefreshTokenExpiresIn!.Value);
        else
            expiresAt = DateTime.UtcNow.AddSeconds(tokenDto.ExpiresIn.Value);

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
