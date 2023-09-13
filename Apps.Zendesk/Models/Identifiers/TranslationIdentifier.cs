using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Identifiers
{
    public class TranslationIdentifier
    {
        [Display("Translation")]
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
