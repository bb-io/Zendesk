using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Zendesk.Models.Blueprints;
public class DownloadContentInput : IDownloadContentInput
{
    [Display("Article ID")]
    [DataSource(typeof(ArticleDataHandler))]
    public string ContentId { get; set; }

    [DataSource(typeof(LocaleDataHandler))]
    [Display("Locale")]
    public string? Locale { get; set; }
}
