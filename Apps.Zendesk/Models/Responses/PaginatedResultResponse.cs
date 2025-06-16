using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses;

public class PaginatedResultResponse<T>
{
    [JsonProperty("next_page")]
    public string? Next { get; set; }

    [JsonProperty("count")]
    public long TotalCount { get; set; }

    public List<T> Results { get; set; } = new();
}
