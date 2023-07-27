using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apps.Zendesk.Dtos
{
    public class ArticleDto
    {
        public long Id { get; set; }

        //public string Url { get; set; }

        public string html_url { get; set; }

        [JsonPropertyName("author_id")]
        public long AuthorId { get; set; }

        [JsonPropertyName("comments_disabled")]
        public bool CommentsDisabled { get; set; }

        [JsonPropertyName("draft")]
        public bool Draft { get; set; }

        [JsonPropertyName("promoted")]
        public bool Promoted { get; set; }

        [JsonPropertyName("outdated")]
        public bool Outdated { get; set; }

        [JsonPropertyName("position")]
        public int Position { get; set; }

        [JsonPropertyName("vote_sum")]
        public int VoteSum { get; set; }

        [JsonPropertyName("vote_count")]
        public int VoteCount { get; set; }

        //public long SectionId { get; set; }

        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonPropertyName("edited_at")]
        public DateTime EditedAt { get; set; }

        //public string Name { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("source_locale")]
        public string SourceLocale { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("outdated_locales")]
        public List<string> OutdatedLocales { get; set; }

        //public long PermissionGroupId { get; set; }

        [JsonPropertyName("body")]
        public string Body { get; set; }
    }
}
