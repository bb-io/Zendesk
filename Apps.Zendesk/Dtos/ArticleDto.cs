using Newtonsoft.Json;

namespace Apps.Zendesk.Dtos
{
    public class ArticleDto
    {
        public long Id { get; set; }

        //public string Url { get; set; }

        public string html_url { get; set; }

        [JsonProperty("author_id")]
        public long AuthorId { get; set; }

        [JsonProperty("comments_disabled")]
        public bool CommentsDisabled { get; set; }

        [JsonProperty("draft")]
        public bool Draft { get; set; }

        [JsonProperty("promoted")]
        public bool Promoted { get; set; }

        [JsonProperty("outdated")]
        public bool Outdated { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("vote_sum")]
        public int VoteSum { get; set; }

        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        //public long SectionId { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updated_at")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("edited_at")]
        public DateTime EditedAt { get; set; }

        //public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("source_locale")]
        public string SourceLocale { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("outdated_locales")]
        public List<string> OutdatedLocales { get; set; }

        //public long PermissionGroupId { get; set; }

        [JsonProperty("body")]
        public string Body { get; set; }
    }
}
