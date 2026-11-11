using Apps.Zendesk.Models.DynamicContent;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Zendesk.Models.Responses;

public class DownloadDynamicContentResponse : IDownloadContentOutput
{
    [Display("Content")]
    public FileReference Content { get; set; } = new();

    [Display("Item")]
    public DynamicContentItem Item { get; set; } = new();

    [Display("Selected variant")]
    public DynamicContentVariant Variant { get; set; } = new();

    [Display("Source language")]
    public string SourceLanguage { get; set; } = string.Empty;

    [Display("Content format")]
    public string ContentFormat { get; set; } = string.Empty;
}
