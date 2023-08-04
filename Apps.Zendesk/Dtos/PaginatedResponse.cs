using Newtonsoft.Json;

namespace Apps.Zendesk.Dtos
{
    public class PaginatedResponse
    {
        [JsonProperty("next_page")]
        public string? NextPage { get; set; }
    }
}
