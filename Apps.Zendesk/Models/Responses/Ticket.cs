using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses;

public class Ticket
{
    [JsonProperty("id")]
    [Display("Ticket ID")]
    public string Id { get; set; }

    [JsonProperty("assignee_id")]
    [Display("Assignee")]
    public string AssigneeId { get; set; }

    [JsonProperty("collaborator_ids")]
    [Display("Collaborator")]
    public IEnumerable<string> CollaboratorIds { get; set; }

    [JsonProperty("created_at")]
    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    [JsonProperty("custom_status_id")]
    [Display("Custom status")]
    public string CustomStatusId { get; set; }

    [JsonProperty("description")]
    [Display("Description")]
    public string Description { get; set; }

    [JsonProperty("due_at")]
    [Display("Due at")]
    public DateTime? DueAt { get; set; }

    [JsonProperty("external_id")]
    [Display("External ID")]
    public string ExternalId { get; set; }

    [JsonProperty("follower_ids")]
    [Display("Followers")]
    public IEnumerable<string> FollowerIds { get; set; }

    [JsonProperty("from_messaging_channel")]
    [Display("From messaging channel")]
    public bool FromMessagingChannel { get; set; }

    [JsonProperty("group_id")]
    [Display("Group")]
    public string GroupId { get; set; }

    [JsonProperty("has_incidents")]
    [Display("Has incidents")]
    public bool HasIncidents { get; set; }

    [JsonProperty("organization_id")]
    [Display("Organization")]
    public string OrganizationId { get; set; }

    [JsonProperty("priority")]
    [Display("Priority")]
    public string Priority { get; set; }

    [JsonProperty("problem_id")]
    [Display("Problem")]
    public string ProblemId { get; set; }

    [JsonProperty("raw_subject")]
    [Display("Raw subject")]
    public string RawSubject { get; set; }

    [JsonProperty("recipient")]
    [Display("Recipient")]
    public string Recipient { get; set; }

    [JsonProperty("requester_id")]
    [Display("Requester")]
    public string RequesterId { get; set; }

    [JsonProperty("sharing_agreement_ids")]
    [Display("Sharing Agreement")]
    public IEnumerable<string> SharingAgreementIds { get; set; }

    [JsonProperty("status")]
    [Display("Status")]
    public string Status { get; set; }

    [JsonProperty("subject")]
    [Display("Subject")]
    public string Subject { get; set; }

    [JsonProperty("submitter_id")]
    [Display("Submitter")]
    public string SubmitterId { get; set; }

    [JsonProperty("tags")]
    [Display("Tags")]
    public IEnumerable<string> Tags { get; set; }

    [JsonProperty("type")]
    [Display("Type")]
    public string Type { get; set; }

    [JsonProperty("updated_at")]
    [Display("Updated at")]
    public DateTime UpdatedAt { get; set; }
}