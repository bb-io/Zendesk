using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses;

public class Translation
{
    [Display("Translation")]
    [JsonProperty("id")]
    public string Id { get; set; }

    [Display("Public URL")]
    [JsonProperty("html_url")]
    public string HtmlUrl { get; set; }

    [Display("Source ID")]
    [JsonProperty("source_id")]
    public string SourceId { get; set; }

    [Display("Locale")]
    [JsonProperty("locale")]
    public string Locale { get; set; }

    [Display("Title")]
    [JsonProperty("title")]
    public string Title { get; set; }

    [Display("Content")]
    [JsonProperty("body")]
    public string Body { get; set; }

    [Display("Is draft?")]
    [JsonProperty("draft")]
    public bool Draft { get; set; }

    [Display("Is outdated?")]
    [JsonProperty("outdated")]
    public bool Outdated { get; set; }

    [Display("Created at")]
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [Display("Updated at")]
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }
}