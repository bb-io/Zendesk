using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Polling.Models;

public class DynamicContentPollingInput
{
    [Display("Source language", Description = "Defaults to each dynamic content item's default language.")]
    [DataSource(typeof(SupportLocaleDataHandler))]
    public string? SourceLanguage { get; set; }

    [Display("Name filter", Description = "Only items whose names contain this value are emitted. Matching is case-insensitive.")]
    public string? NameFilter { get; set; }
}
