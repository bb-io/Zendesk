namespace Apps.Zendesk.Models.Input.Ticket;

public class CreateTicketInput
{
    public virtual string Comment { get; set; }
    public string? Priority { get; set; }
    public string? Subject { get; set; }
}