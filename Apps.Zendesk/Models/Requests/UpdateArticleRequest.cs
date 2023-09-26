using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Requests;

public class UpdateArticleRequest
{
    // Todo: content tags, label names

    [Display("Permission group")]
    [DataSource(typeof(PermissionGroupDataHandler))]
    [JsonProperty("permission_group_id")]
    public string? PermissionGroupId { get; set; }

    [Display("User segment")]
    [DataSource(typeof(UserSegmentDataHandler))]
    [JsonProperty("user_segment_id")]
    public string? UserSegmentId { get; set; }

    [Display("Author")]
    [DataSource(typeof(AdminDataHandler))]
    [JsonProperty("author_id")]
    public string? AuthorId { get; set; }

    [Display("Section")]
    [DataSource(typeof(SectionDataHandler))]
    [JsonProperty("section_id")]
    public string? SectionId { get; set; }

    [Display("Are comments disabled?")]
    [JsonProperty("comments_disabled")]
    public bool? CommentsDisabled { get; set; }

    [Display("Is promoted?")]
    [JsonProperty("promoted")]
    public bool? Promoted { get; set; }

    [Display("Position")]
    [JsonProperty("position")]
    public int? Position { get; set; }        
}