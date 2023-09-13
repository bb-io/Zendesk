using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Requests
{
    public class SectionRequest
    {
        [Display("Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Display("Description")]
        [JsonProperty("description")]
        public string? Description { get; set; }

        [Display("Position")]
        [JsonProperty("position")]
        public int? Position { get; set; }
    }
}
