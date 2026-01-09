namespace Apps.Zendesk.Models.Dtos;

public class OAuth2TokenResponse
{
    private const int DefaultTokenExpirationSeconds = 3600;

    public string AccessToken { get; init; } = null!;
    public string? RefreshToken { get; init; }
    public string? TokenType { get; init; }
    public string? Scope { get; init; }
    public int ExpiresIn { get; init; }
    public DateTime ExpiresAt { get; init; }
    public int? RefreshTokenExpiresIn { get; init; }

    public Dictionary<string, string?> ToDictionary()
    {
        return new Dictionary<string, string?>
        {
            { "access_token", AccessToken },
            { "refresh_token", RefreshToken },
            { "token_type", TokenType },
            { "scope", Scope },
            { "expires_in", ExpiresIn.ToString() },
            { "expires_at", ExpiresAt.ToString("O") },
            { "refresh_token_expires_in", RefreshTokenExpiresIn?.ToString() }
        };
    }

    public static OAuth2TokenResponse FromTokenDto(TokenDto tokenDto)
    {
        var expiresIn = tokenDto.ExpiresIn ?? DefaultTokenExpirationSeconds;
        
        return new OAuth2TokenResponse
        {
            AccessToken = tokenDto.AccessToken,
            RefreshToken = tokenDto.RefreshToken,
            TokenType = tokenDto.TokenType,
            Scope = tokenDto.Scope,
            ExpiresIn = expiresIn,
            ExpiresAt = DateTime.UtcNow.AddSeconds(expiresIn),
            RefreshTokenExpiresIn = tokenDto.RefreshTokenExpiresIn
        };
    }
}
