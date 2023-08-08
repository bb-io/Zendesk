using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Requests
{
    public class TranslateArticleRequest
    {
        [DataSource(typeof(LocaleDataHandler))]
        public string Locale { get; set; }

        [Display("Article")]
        [DataSource(typeof(ArticleDataHandler))]
        public string ArticleId { get; set; }

        public string Title { get; set; }

        [Display("Content (HTML)")]
        public string Body { get; set; }

        public bool? Draft { get; set; }
    }
}
