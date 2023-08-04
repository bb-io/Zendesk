namespace Apps.Zendesk.Dtos;

public class TicketsResponseWrapper : PaginatedResponse
{
    public IEnumerable<TicketDto> Tickets { get; set; }
}