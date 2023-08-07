using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Dtos;

public class ChatDto
{
    [JsonProperty("agent_ids")]
    [Display("Agent IDs")]
    public IEnumerable<string> AgentIds { get; set; }

    [JsonProperty("agent_names")]
    [Display("Agent names")]
    public IEnumerable<string> AgentNames { get; set; }

    [JsonProperty("comment")]
    [Display("Comment")]
    public string Comment { get; set; }

    [JsonProperty("department_id")]
    [Display("Department ID")]
    public string DepartmentId { get; set; }

    [JsonProperty("department_name")]
    [Display("Department name")]
    public string DepartmentName { get; set; }

    [JsonProperty("duration")]
    [Display("Duration")]
    public int Duration { get; set; }

    [JsonProperty("id")] [Display("ID")] public string Id { get; set; }

    [JsonProperty("rating")]
    [Display("Rating")]
    public string Rating { get; set; }

    [JsonProperty("started_by")]
    [Display("Started By")]
    public string StartedBy { get; set; }

    [JsonProperty("timestamp")]
    [Display("Timestamp")]
    public DateTime Timestamp { get; set; }

    [JsonProperty("triggered")]
    [Display("Triggered")]
    public bool Triggered { get; set; }

    [JsonProperty("type")]
    [Display("Type")]
    public string Type { get; set; }

    [JsonProperty("unread")]
    [Display("Unread")]
    public bool Unread { get; set; }
}