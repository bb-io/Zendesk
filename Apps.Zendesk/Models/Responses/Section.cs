using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses;

public class Section
{
    [JsonProperty("category_id")]
    [Display("Category")]
    public string CategoryId { get; set; }

    [JsonProperty("id")]
    [Display("Section")]
    public string Id { get; set; }

    [JsonProperty("html_url")]
    [Display("Public URL")]
    public string HtmlUrl { get; set; }

    [JsonProperty("position")]
    [Display("Position")]
    public int Position { get; set; }

    [JsonProperty("created_at")]
    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("updated_at")]
    [Display("Updated at")]
    public DateTime UpdatedAt { get; set; }

    [JsonProperty("name")]
    [Display("Name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    [Display("Description")]
    public string Description { get; set; }

    [JsonProperty("locale")]
    [Display("Locale")]
    public string Locale { get; set; }

    [JsonProperty("source_locale")]
    [Display("Source locale")]
    public string SourceLocale { get; set; }

    [JsonProperty("theme_template")]
    [Display("Theme template")]
    public string ThemeTemplate { get; set; }

    [JsonProperty("outdated")]
    [Display("Is outdated?")]
    public bool Outdated { get; set; }
}