using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Requests;

public class DownloadDynamicContentRequest
{
    [Display("Dynamic content ID")]
    public string DynamicContentId { get; set; } = string.Empty;

    [Display("Source language", Description = "Defaults to the dynamic content item's default language.")]
    [DataSource(typeof(SupportLocaleDataHandler))]
    public string? SourceLanguage { get; set; }

    [Display("Content format", Description = "Defaults to Text.")]
    [DataSource(typeof(DynamicContentFormatDataHandler))]
    public string? ContentFormat { get; set; }
}
