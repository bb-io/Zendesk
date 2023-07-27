using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Requests
{
    public class TranslateArticleRequest
    {
        public string Locale { get; set; }

        [Display("Article ID")]
        public string ArticleId { get; set; }

        public string Title { get; set; }

        [Display("Content (HTML)")]
        public string Body { get; set; }

        public bool? Draft { get; set; }
    }
}
