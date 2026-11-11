using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Zendesk.Models.Responses;

public class UploadDynamicContentResponse : IDownloadContentOutput
{
    [Display("Content")]
    public FileReference Content { get; set; } = new();

    [Display("Dynamic content ID")]
    public string DynamicContentId { get; set; } = string.Empty;

    [Display("Variant ID")]
    public string VariantId { get; set; } = string.Empty;

    [Display("Target language")]
    public string TargetLanguage { get; set; } = string.Empty;

    [Display("Operation")]
    public string Operation { get; set; } = string.Empty;
}
