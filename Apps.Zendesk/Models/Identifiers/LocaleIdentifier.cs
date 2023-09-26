using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Identifiers;

public class LocaleIdentifier
{
    [DataSource(typeof(LocaleDataHandler))]
    [Display("Locale")]
    public string Locale { get; set; }
}