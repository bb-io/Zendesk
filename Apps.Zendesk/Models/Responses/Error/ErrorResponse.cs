using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses.Error;

public class ErrorResponse
{
    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }
}