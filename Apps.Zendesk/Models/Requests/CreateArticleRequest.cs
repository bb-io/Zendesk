using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Requests;

public class CreateArticleRequest
{
    // Todo: content tags, label names

    [Display("Title")]
    [JsonProperty("title")]
    public string Title { get; set; }

    [Display("Permission group")]
    [DataSource(typeof(PermissionGroupDataHandler))]
    [JsonProperty("permission_group_id")]
    public string PermissionGroupId { get; set; }

    [Display("User segment")]
    [DataSource(typeof(UserSegmentDataHandler))]
    [JsonProperty("user_segment_id")]
    public string UserSegmentId { get; set; }

    [Display("Content (HTML)")]
    [JsonProperty("body")]
    public string? Body { get; set; }

    [Display("Author")]
    [DataSource(typeof(AdminDataHandler))]
    [JsonProperty("author_id")]
    public string? AuthorId { get; set; }

    [Display("Are comments disabled?")]
    [JsonProperty("comments_disabled")]
    public bool? CommentsDisabled { get; set; }

    [Display("Is draft?")]
    [JsonProperty("draft")]
    public bool? Draft { get; set; }

    [Display("Is promoted?")]
    [JsonProperty("promoted")]
    public bool? Promoted { get; set; }

    [Display("Position")]
    [JsonProperty("position")]
    public int? Position { get; set; }

}