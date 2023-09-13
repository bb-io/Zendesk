using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses
{
    public class NamedResource
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
