using Newtonsoft.Json;

namespace Apps.Zendesk.Dtos
{
    public class TranslationDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("html_url")]
        public string html_url { get; set; }

        [JsonProperty("source_id")]
        public string SourceId { get; set; }

        [JsonProperty("source_type")]
        public string SourceType { get; set; }

        public string Locale { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public bool Draft { get; set; }
        public bool Hidden { get; set; }
        public bool Outdated { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }
    }
}
