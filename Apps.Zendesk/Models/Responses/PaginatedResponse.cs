using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses
{
    public class Links
    {
        [JsonProperty("next")]
        public string? Next { get; set; }
    }

    public class Meta
    {
        [JsonProperty("has_more")]
        public bool HasMore { get; set; }
    }

    public class PaginatedResponse
    {
        [JsonProperty("links")]
        public Links? Links { get; set; }

        [JsonProperty("meta")]
        public Meta? Meta { get; set; }

        [JsonProperty("next_page")]
        public string? Next { get; set; }

        public string? NextPage => Meta != null ? (Meta.HasMore ? Links?.Next : null) : Next;
    }
}
