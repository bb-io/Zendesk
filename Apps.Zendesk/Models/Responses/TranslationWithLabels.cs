using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Apps.Zendesk.Models.Responses
{
    public class TranslationWithLabels : Translation
    {
        [Display("Labels")]
        public IEnumerable<string> Labels { get; set; }

        public TranslationWithLabels(Translation article, IEnumerable<string> labels)
        {
            Id = article.Id;
            HtmlUrl = article.HtmlUrl;
            Draft = article.Draft;
            Outdated = article.Outdated;
            CreatedAt = article.CreatedAt;
            UpdatedAt = article.UpdatedAt;
            Title = article.Title;
            Locale = article.Locale;
            Body = article.Body;
            SourceId = article.SourceId;

            Labels = labels;
        }
    }
}
