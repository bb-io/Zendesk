namespace Apps.Zendesk.Models.Requests.Ticket;

public class TicketRequest
{
    public RequestEntity Comment { get; set; }
    public string? Priority { get; set; }
    public string? Subject { get; set; }
}