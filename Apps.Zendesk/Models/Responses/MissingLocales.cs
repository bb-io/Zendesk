using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses;

public class MissingLocales
{
    [Display("Locales")]
    [JsonProperty("locales")]
    public List<string> Locales { get; set; }
}