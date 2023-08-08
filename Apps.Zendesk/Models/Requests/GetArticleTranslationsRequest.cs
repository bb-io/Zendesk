using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Requests
{
    public class GetArticleTranslationsRequest
    {
        [Display("Article")]
        [DataSource(typeof(ArticleDataHandler))]
        public string ArticleId { get; set; }
    }
}
