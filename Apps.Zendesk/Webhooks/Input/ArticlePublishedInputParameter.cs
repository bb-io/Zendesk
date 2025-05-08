using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Webhooks.Input;

public class ArticlePublishedInputParameter
{
    [Display("Only source articles")]
    public bool? OnlyIfSource { get; set; }

    [Display("Brand ID")]
    public string? BrandId { get; set; }

    [Display("Account ID")]
    public string? AccountId { get; set; }

    [Display("Required label")]
    [DataSource(typeof(LabelNameDataHandler))]
    public string? RequiredLabel { get; set; }

    [Display("Language")]
    [DataSource(typeof(LocaleDataHandler))]
    public string? Locale { get; set; }

    [Display("Article ID")]
    [DataSource(typeof(ArticleDataHandler))]
    public string? ArticleId { get; set; }
}
