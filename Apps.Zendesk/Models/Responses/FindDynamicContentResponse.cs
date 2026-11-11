using Apps.Zendesk.Models.DynamicContent;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Responses;

public class FindDynamicContentResponse
{
    [Display("Found")]
    public bool Found { get; set; }

    [Display("Item")]
    public DynamicContentItem? Item { get; set; }
}
