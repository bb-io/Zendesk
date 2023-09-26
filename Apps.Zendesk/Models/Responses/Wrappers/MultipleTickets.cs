namespace Apps.Zendesk.Models.Responses.Wrappers;

public class MultipleTickets : PaginatedResponse
{
    public IEnumerable<Ticket> Tickets { get; set; }
}