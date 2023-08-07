using Apps.Zendesk.Models.Input.Ticket;

namespace Apps.Zendesk.Models.Requests.Ticket;

public class ManageTicketRequest
{
    public TicketRequest Ticket { get; set; }

    public ManageTicketRequest(CreateTicketInput input)
    {
        Ticket = new()
        {
            Comment = new()
            {
                Body = input.Comment
            },
            Priority = input.Priority,
            Subject = input.Subject
        };
    }
}