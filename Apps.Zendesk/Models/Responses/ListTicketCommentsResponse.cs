using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses
{
    public class ListTicketCommentsResponse
    {
        public IEnumerable<TicketComment> Comments { get; set; } = Array.Empty<TicketComment>();
    }

    public class TicketComment
    {
        [Display("Ticket ID")]
        public string Id { get; set; }
        public string? Type { get; set; }

        [Display("Author ID")]
        [JsonProperty("author_id")]
        public string AuthorId { get; set; }

        [JsonProperty("body")]
        public string? Body { get; set; }

        [Display("HTML body")]
        [JsonProperty("html_body")]
        public string? HtmlBody { get; set; }

        [Display("Plain body")]
        [JsonProperty("plain_body")]
        public string? PlainBody { get; set; }

        public bool Public { get; set; }

        public List<TicketAttachment> Attachments { get; set; } = new();

        [Display("Audit ID")]
        [JsonProperty("audit_id")]
        public long AuditId { get; set; }

        public TicketVia? Via { get; set; }

        [Display("Created at")]
        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        public TicketCommentMetadata? Metadata { get; set; }
    }

    public class TicketAttachment
    {
        public string Id { get; set; }
        [Display("File name")]
        [JsonProperty("file_name")]
        public string? FileName { get; set; }

        [Display("Content URL")]
        [JsonProperty("content_url")]
        public string? ContentUrl { get; set; }

        [Display("Content type")]
        [JsonProperty("content_type")]
        public string? ContentType { get; set; }
        public long? Size { get; set; }
        public bool? Inline { get; set; }
    }

    public class TicketVia
    {
        public string? Channel { get; set; }
        public TicketViaSource? Source { get; set; }
    }

    public class TicketViaSource
    {
        public Dictionary<string, object>? From { get; set; }
        public TicketViaEndpoint? To { get; set; }
        public string? Rel { get; set; }
    }
    public class TicketViaEndpoint
    {
        public string? Address { get; set; }
        public string? Name { get; set; }
    }
    public class TicketCommentMetadata
    {
        public TicketCommentSystemMetadata? System { get; set; }
        public Dictionary<string, object>? Custom { get; set; }
    }

    public class TicketCommentSystemMetadata
    {
        public string? Client { get; set; }

        [Display("IP address")]
        [JsonProperty("ip_address")]
        public string? IpAddress { get; set; }

        public string? Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
    }
}
