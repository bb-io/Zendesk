using Apps.Zendesk.Webhooks.Payload.Articles;
using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses
{
    public class ListCommentsResponse
    {
        public IEnumerable<ArticleComment> Comments { get; set; } = Array.Empty<ArticleComment>();
    }

    public class ArticleComment
    {
        [Display("ID")]
        public long Id { get; set; }

        public string? Body { get; set; }

        [Display("Author ID")]
        public long AuthorId { get; set; }

        public string? Locale { get; set; }

        [Display("Created at")]
        public DateTime? CreatedAt { get; set; }

        [Display("Updated at")]
        public DateTime? UpdatedAt { get; set; }

        [Display("HTML URL")]
        public string? HtmlUrl { get; set; }

        [Display("Source ID")]
        public long? SourceId { get; set; }

        [Display("Source type")]
        public string? SourceType { get; set; }

        public string? Url { get; set; }

        [Display("Vote count")]
        public int? VoteCount { get; set; }

        [Display("Vote sum")]
        public int? VoteSum { get; set; }

        [Display("Non-author editor ID")]
        public long? NonAuthorEditorId { get; set; }

        [Display("Non-author updated at")]
        public DateTime? NonAuthorUpdatedAt { get; set; }
    }
}
