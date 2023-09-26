using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses;

public class Article
{
    [Display("Article ID")]
    [JsonProperty("id")]
    public string Id { get; set; }

    [Display("Public URL")]
    [JsonProperty("html_url")]
    public string HtmlUrl { get; set; }

    [Display("Author")]
    [JsonProperty("author_id")]
    public string AuthorId { get; set; }

    [Display("Are comments disabled?")]
    [JsonProperty("comments_disabled")]
    public bool CommentsDisabled { get; set; }

    [Display("Is draft?")]
    [JsonProperty("draft")]
    public bool Draft { get; set; }

    [Display("Is promoted?")]
    [JsonProperty("promoted")]
    public bool Promoted { get; set; }

    [Display("Is outdated?")]
    [JsonProperty("outdated")]
    public bool Outdated { get; set; }

    [Display("Position")]
    [JsonProperty("position")]
    public int Position { get; set; }

    [Display("Sum of votes")]
    [JsonProperty("vote_sum")]
    public int VoteSum { get; set; }

    [Display("Number of votes")]
    [JsonProperty("vote_count")]
    public int VoteCount { get; set; }

    [Display("Section")]
    [JsonProperty("section_id")]
    public string SectionId { get; set; }

    [Display("Created at")]
    [JsonProperty("created_at")]
    public DateTime CreatedAt { get; set; }

    [Display("Updated at")]
    [JsonProperty("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Display("Edited at")]
    [JsonProperty("edited_at")]
    public DateTime EditedAt { get; set; }

    [Display("Name")]
    [JsonProperty("name")]
    public string Name { get; set; }

    [Display("Title")]
    [JsonProperty("title")]
    public string Title { get; set; }

    [Display("Source locale")]
    [JsonProperty("source_locale")]
    public string SourceLocale { get; set; }

    [Display("Locale")]
    [JsonProperty("locale")]
    public string Locale { get; set; }

    [Display("Outdated locales")]
    [JsonProperty("outdated_locales")]
    public List<string> OutdatedLocales { get; set; }

    //public long PermissionGroupId { get; set; }

    [Display("Content (HTML)")]
    [JsonProperty("body")]
    public string Body { get; set; }        
}