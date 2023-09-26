using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses;

public class EnabledLocales
{
    [Display("Locales")]
    [JsonProperty("locales")]
    public List<string> Locales { get; set; }

    [Display("Default locale")]
    [JsonProperty("default_locale")]
    public string DefaultLocale { get; set; }
}