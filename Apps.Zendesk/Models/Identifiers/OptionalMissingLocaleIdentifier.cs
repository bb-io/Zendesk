using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Identifiers;

public class OptionalMissingLocaleIdentifier
{
    [DataSource(typeof(LocaleDataHandler))]
    [Display("Missing translation in")]
    public string? Locale { get; set; }
}