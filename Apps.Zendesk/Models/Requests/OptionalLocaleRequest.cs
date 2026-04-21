using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Requests;

public class OptionalLocaleRequest
{
    [Display("Locale")]
    [DataSource(typeof(LocaleDataHandler))]
    public string? Locale { get; set; }
}
