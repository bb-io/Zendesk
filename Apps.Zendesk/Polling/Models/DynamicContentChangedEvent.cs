using Apps.Zendesk.Models.DynamicContent;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Zendesk.Polling.Models;

public class DynamicContentChangedEvent : IDownloadContentInput
{
    [Display("Dynamic content ID")]
    public string DynamicContentId { get; set; } = string.Empty;

    [Display("Name")]
    public string Name { get; set; } = string.Empty;

    [Display("Placeholder")]
    public string Placeholder { get; set; } = string.Empty;

    [Display("Item")]
    public DynamicContentItem Item { get; set; } = new();

    [Display("Selected variant")]
    public DynamicContentVariant Variant { get; set; } = new();

    [Display("Source language")]
    public string SourceLanguage { get; set; } = string.Empty;

    [Display("Variant ID")]
    public string VariantId { get; set; } = string.Empty;

    [Display("Text")]
    public string Text { get; set; } = string.Empty;

    [Display("Event type", Description = "Either created or updated.")]
    public string EventType { get; set; } = string.Empty;

    string IDownloadContentInput.ContentId
    {
        get => DynamicContentId;
        set => DynamicContentId = value;
    }
}
