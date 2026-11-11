using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Zendesk.Models.Requests;

public class UploadDynamicContentRequest : IUploadContentInput
{
    [Display("Content file")]
    public FileReference Content { get; set; } = new();

    [Display("Target language")]
    [DataSource(typeof(SupportLocaleDataHandler))]
    public string? TargetLanguage { get; set; }

    [Display("Dynamic content ID")]
    public string? DynamicContentId { get; set; }

    [Display("Active")]
    public bool? Active { get; set; }

    string IUploadContentInput.Locale
    {
        get => TargetLanguage ?? string.Empty;
        set => TargetLanguage = value;
    }

    string? IUploadContentInput.ContentId
    {
        get => DynamicContentId;
        set => DynamicContentId = value;
    }
}
