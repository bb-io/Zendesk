using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Identifiers;

public class ArticleIdentifier
{
    [Display("Article ID")]
    [DataSource(typeof(ArticleDataHandler))]
    public string ContentId { get; set; }
}