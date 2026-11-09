using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.DynamicContent;

public class DynamicContentVariant
{
    [Display("Variant ID")]
    [JsonProperty("id")]
    public string Id { get; set; } = string.Empty;

    [Display("Locale ID")]
    [JsonProperty("locale_id")]
    public long LocaleId { get; set; }

    [Display("Text")]
    [JsonProperty("content")]
    public string Content { get; set; } = string.Empty;

    [Display("Is active?")]
    [JsonProperty("active")]
    public bool Active { get; set; }

    [Display("Is default?")]
    [JsonProperty("default")]
    public bool Default { get; set; }

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
}
