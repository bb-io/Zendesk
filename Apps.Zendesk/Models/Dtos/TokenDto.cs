using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Zendesk.Models.Dtos;

internal class TokenDto 
{
    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = null!;

    [JsonProperty("refresh_token")]
    public string? RefreshToken { get; set; }

    [JsonProperty("token_type")]
    public string? TokenType { get; set; }

    [JsonProperty("scope")]
    public string? Scope { get; set; }

    [JsonProperty("refresh_token_expires_in")]
    public int? RefreshTokenExpiresIn { get; set; }


    [JsonExtensionData]
    public Dictionary<string, JToken>? AdditionalData { get; set; }
}
