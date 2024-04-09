using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Webhooks.Responses;

public class ArticleResponse
{
    [Display("Article ID")]
    [DataSource(typeof(ArticleDataHandler))]
    public string ArticleId { get; set; }
}