using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Requests.Chat;

public class VisitorData
{
    [JsonProperty("phone")]
    public string? Phone { get; set; }

    [JsonProperty("notes")]
    public string? Notes { get; set; }

    [JsonProperty("ID")]
    public string? Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }
}