using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Identifiers;

public class SectionIdentifier
{
    [Display("Section")]
    [DataSource(typeof(SectionDataHandler))]
    public string Id { get; set; }
}