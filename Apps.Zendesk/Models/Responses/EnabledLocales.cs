using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses;

public class EnabledLocales
{
    [Display("Default language")]
    [JsonProperty("default_locale")]
    public string DefaultLocale { get; set; }

    [Display("All languages")]
    [JsonProperty("locales")]
    public List<string> Locales { get; set; }

    [Display("Additional languages", Description = "All languages except the default language")]
    public List<string> AdditionalLocales => Locales.Where(x => x != DefaultLocale).ToList();

}