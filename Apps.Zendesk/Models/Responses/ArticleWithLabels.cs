using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses
{
    public class ArticleWithLabels : Article
    {
        [Display("Labels")]
        public IEnumerable<string> Labels { get; set; }

        public ArticleWithLabels(Article article, IEnumerable<string> labels)
        {
            ContentId = article.ContentId;            
            HtmlUrl = article.HtmlUrl;
            AuthorId = article.AuthorId;
            CommentsDisabled = article.CommentsDisabled;
            Draft = article.Draft;
            Promoted = article.Promoted;
            Outdated = article.Outdated;
            Position = article.Position;
            VoteSum = article.VoteSum;
            VoteCount = article.VoteCount;
            SectionId = article.SectionId;
            CreatedAt = article.CreatedAt;
            UpdatedAt = article.UpdatedAt;
            EditedAt = article.EditedAt;
            Name = article.Name;
            Title = article.Title;
            SourceLocale = article.SourceLocale;
            Locale = article.Locale;
            OutdatedLocales = article.OutdatedLocales;
            Body = article.Body;

            Labels = labels;
        }
    }
}
