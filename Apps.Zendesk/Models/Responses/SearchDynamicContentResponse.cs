using Apps.Zendesk.Models.DynamicContent;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Responses;

public class SearchDynamicContentResponse
{
    [Display("Items")]
    public List<DynamicContentItem> Items { get; set; } = [];

    [Display("Total count")]
    public int TotalCount { get; set; }
}
