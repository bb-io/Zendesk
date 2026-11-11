using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Requests;

public class SearchDynamicContentRequest
{
    [Display("Query", Description = "Matches dynamic content names or placeholders case-insensitively. Leave empty to return all items.")]
    public string? Query { get; set; }
}
