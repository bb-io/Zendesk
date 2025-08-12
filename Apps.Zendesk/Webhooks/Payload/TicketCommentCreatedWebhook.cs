using Newtonsoft.Json;

namespace Apps.Zendesk.Webhooks.Payload
{
    public class TicketCommentCreatedWebhook
    {
        [JsonProperty("account_id")]
        public string AccountId { get; set; }

        [JsonProperty("detail")]
        public TicketCommentDetail Detail { get; set; } = null!;

        [JsonProperty("event")]
        public TicketCommentEvent Event { get; set; } = null!;

        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("subject")]
        public string Subject { get; set; } = null!;

        [JsonProperty("time")]
        public DateTime Time { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; } = null!;

        [JsonProperty("zendesk_event_version")]
        public string ZendeskEventVersion { get; set; } = null!;
    }

    public class TicketCommentDetail
    {
        [JsonProperty("actor_id")]
        public string ActorId { get; set; } = null!;

        [JsonProperty("assignee_id")]
        public string AssigneeId { get; set; } = null!;

        [JsonProperty("brand_id")]
        public string BrandId { get; set; } = null!;

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("custom_status")]
        public string? CustomStatus { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; } = null!;

        [JsonProperty("external_id")]
        public string? ExternalId { get; set; }

        [JsonProperty("form_id")]
        public string? FormId { get; set; }

        [JsonProperty("group_id")]
        public string? GroupId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }

        [JsonProperty("organization_id")]
        public string? OrganizationId { get; set; }

        [JsonProperty("priority")]
        public string? Priority { get; set; }

        [JsonProperty("requester_id")]
        public string RequesterId { get; set; } = null!;

        [JsonProperty("status")]
        public string Status { get; set; } = null!;

        [JsonProperty("subject")]
        public string Subject { get; set; } = null!;

        [JsonProperty("submitter_id")]
        public string SubmitterId { get; set; } = null!;

        [JsonProperty("tags")]
        public List<string> Tags { get; set; } = new();

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("via")]
        public TicketVia Via { get; set; } = null!;
    }

    public class TicketVia
    {
        [JsonProperty("channel")]
        public string Channel { get; set; } = null!;
    }

    public class TicketCommentEvent
    {
        [JsonProperty("comment")]
        public TicketCommentPayload Comment { get; set; } = null!;
    }

    public class TicketCommentPayload
    {
        [JsonProperty("author")]
        public TicketCommentAuthor Author { get; set; } = null!;

        [JsonProperty("body")]
        public string Body { get; set; } = null!;

        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("is_public")]
        public bool IsPublic { get; set; }
    }

    public class TicketCommentAuthor
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("is_staff")]
        public bool IsStaff { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = null!;
    }
}
