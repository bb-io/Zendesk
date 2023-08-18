using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Identifiers;

public class TicketIdentifier
{
    [Display("Ticket")]
    [DataSource(typeof(TicketDataHandler))]
    public string Id { get; set; }
}