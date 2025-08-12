using Apps.Zendesk.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Responses
{
    public class TicketCommentCreatedEventDto
    {
        [Display("Account ID")]
        public string AccountId { get; set; }
        public string Subject { get; set; } = null!;
        public DateTime Time { get; set; }

        [Display("Brand ID")]
        public string BrandId { get; set; } = null!;

        [Display("Ticket ID")]
        public string TicketId { get; set; } = null!;

        [Display("Organization ID")]
        public string? OrganizationId { get; set; }
        public string Status { get; set; } = null!;

        [Display("Comment author ID")]
        public string CommentAuthorId { get; set; } = null!;

        [Display("Comment author name")]
        public string CommentAuthorName { get; set; } = null!;

        [Display("Comment body")]
        public string CommentBody { get; set; } = null!;

        [Display("Comment ID")]
        public string CommentId { get; set; } = null!;

        [Display("Is comment public")]
        public bool CommentIsPublic { get; set; }

        public TicketCommentCreatedEventDto(TicketCommentCreatedWebhook source)
        {
            AccountId = source.AccountId;
            Subject = source.Subject;
            Time = source.Time;
            BrandId = source.Detail.BrandId;
            TicketId = source.Detail.Id;
            OrganizationId = source.Detail.OrganizationId;
            CommentAuthorId = source.Event.Comment.Author.Id;
            CommentAuthorName = source.Event.Comment.Author.Name;
            CommentBody = source.Event.Comment.Body;
            CommentId = source.Event.Comment.Id;
            CommentIsPublic = source.Event.Comment.IsPublic;
        }
    }
}
