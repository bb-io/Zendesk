using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Requests
{
    public class GetArticleRequest
    {
        [Display("Article")]
        [DataSource(typeof(ArticleDataHandler))]
        public string ArticleId { get; set; }

        [DataSource(typeof(LocaleDataHandler))]
        public string? Locale { get; set; }
    }
}
