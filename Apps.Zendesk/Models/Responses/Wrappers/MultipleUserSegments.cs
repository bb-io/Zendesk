using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses.Wrappers
{
    public class MultipleUserSegments : PaginatedResponse
    {
        [JsonProperty("user_segments")]
        public IEnumerable<NamedResource> UserSegments { get; set; }
    }
}
