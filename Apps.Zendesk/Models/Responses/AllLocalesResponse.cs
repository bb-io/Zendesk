using Blackbird.Applications.Sdk.Common;
using System.Text.Json.Serialization;

namespace Apps.Zendesk.Models.Responses
{
    public class AllLocalesResponse
    {
        [Display("Locales")]
        public List<string> Locales { get; set; }

        [Display("Default locale")]
        [JsonPropertyName("default_locale")]
        public string DefaultLocale { get; set; }
    }
}
