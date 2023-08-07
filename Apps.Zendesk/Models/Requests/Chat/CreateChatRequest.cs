using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Requests.Chat;

public class CreateChatRequest
{
    [JsonProperty("visitor")]
    public VisitorData? Visitor { get; set; }

    [JsonProperty("message")]
    public string? Message { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("timestamp")]
    public long? Timestamp { get; set; }

    [JsonProperty("session")]
    public SessionData? Session { get; set; }
}