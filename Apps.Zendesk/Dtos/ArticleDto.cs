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
        public long Id { get; set; }

        //public string Url { get; set; }

        public string html_url { get; set; }

        public long AuthorId { get; set; }

        public bool CommentsDisabled { get; set; }

        public bool Draft { get; set; }

        public bool Promoted { get; set; }

        public bool Outdated { get; set; }

        public int Position { get; set; }

        public int VoteSum { get; set; }

        public int VoteCount { get; set; }

        //public long SectionId { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime EditedAt { get; set; }

        //public string Name { get; set; }
        public string Title { get; set; }
        public string SourceLocale { get; set; }
        public string Locale { get; set; }

        //public long PermissionGroupId { get; set; }
        public string Body { get; set; }
    }
}
