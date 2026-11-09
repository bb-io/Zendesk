using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Requests;

public class FindDynamicContentRequest
{
    [Display("Dynamic content ID")]
    public string? DynamicContentId { get; set; }

    [Display("Placeholder", Description = "Accepts values such as hc_toggle_theme, dc.hc_toggle_theme, or {{dc.hc_toggle_theme}}.")]
    public string? Placeholder { get; set; }
}
