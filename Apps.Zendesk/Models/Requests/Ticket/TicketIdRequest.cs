using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Requests.Ticket;

public class TicketIdRequest
{
    [Display("Ticket")]
    [DataSource(typeof(TicketDataHandler))]
    public string TicketId { get; set; }
}