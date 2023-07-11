using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Dtos
{
    public class ArticleDto
    {

        [Display("Article ID")]
        public long Id { get; set; }

        //public string Url { get; set; }

        [Display("Public URL")]
        public string html_url { get; set; }

        [Display("Path")]
        public string Path => new Uri(html_url).AbsolutePath;

        [Display("Author iD")]
        public long AuthorId { get; set; }

        [Display("Comments disabled?")]
        public bool CommentsDisabled { get; set; }

        [Display("Is draft?")]
        public bool Draft { get; set; }

        [Display("Is promoted?")]
        public bool Promoted { get; set; }

        [Display("Is outdated?")]
        public bool Outdated { get; set; }

        public int Position { get; set; }

        [Display("Sum of votes")]
        public int VoteSum { get; set; }

        [Display("Number of votes")]
        public int VoteCount { get; set; }

        //public long SectionId { get; set; }

        [Display("Created at")]
        public DateTime CreatedAt { get; set; }

        [Display("Updated at")]
        public DateTime UpdatedAt { get; set; }

        [Display("Edited at")]
        public DateTime EditedAt { get; set; }

        //public string Name { get; set; }
        public string Title { get; set; }

        [Display("Source locale")]
        public string SourceLocale { get; set; }
        public string Locale { get; set; }

        //public long PermissionGroupId { get; set; }

        [Display("Content (HTML)")]
        public string Body { get; set; }
    }
}
