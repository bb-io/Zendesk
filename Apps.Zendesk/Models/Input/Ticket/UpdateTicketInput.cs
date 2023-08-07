namespace Apps.Zendesk.Models.Input.Ticket;

public class UpdateTicketInput : CreateTicketInput
{
    public override string? Comment { get; set; }
}