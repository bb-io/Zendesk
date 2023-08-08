using Apps.Zendesk.Dtos;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Responses
{
    public class Article
    {
        [Display("Article")]
        public string Id { get; set; }

        //public string Url { get; set; }

        [Display("Public URL")]
        public string html_url { get; set; }

        [Display("Author iD")]
        public string AuthorId { get; set; }

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

        [Display("Outdated locales")]
        public List<string> OutdatedLocales { get; set; }

        //public long PermissionGroupId { get; set; }

        [Display("Content (HTML)")]
        public string Body { get; set; }

        public static Article? FromDto(ArticleDto? dto)
        {
            if (dto == null) return null;
            return new Article
            {
                Id = dto.Id.ToString(),
                AuthorId = dto.AuthorId.ToString(),
                html_url = dto.html_url,
                CommentsDisabled= dto.CommentsDisabled,
                Draft = dto.Draft,
                Promoted= dto.Promoted, 
                Outdated= dto.Outdated,
                Position= dto.Position,
                VoteCount= dto.VoteCount,
                VoteSum= dto.VoteSum,
                CreatedAt= dto.CreatedAt,
                EditedAt= dto.EditedAt,
                Title= dto.Title,
                SourceLocale = dto.SourceLocale,
                Locale= dto.Locale,
                OutdatedLocales= dto.OutdatedLocales,
                Body= dto.Body,
            };
        }
    }
}
