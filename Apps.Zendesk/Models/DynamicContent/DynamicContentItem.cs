using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.DynamicContent;

public class DynamicContentItem
{
    [Display("Dynamic content ID")]
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [Display("Name")]
    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;

    [Display("Placeholder")]
    [JsonProperty("placeholder")]
    public string Placeholder { get; set; } = string.Empty;

    [Display("Default locale ID")]
    [JsonProperty("default_locale_id")]
    public long DefaultLocaleId { get; set; }

    [Display("Is outdated?")]
    [JsonProperty("outdated")]
    public bool Outdated { get; set; }

    [Display("Created at")]
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [Display("Updated at")]
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Display("URL")]
    [JsonProperty("url")]
    public string Url { get; set; } = string.Empty;

    [Display("Variants")]
    [JsonProperty("variants")]
    public List<DynamicContentVariant> Variants { get; set; } = [];
}
