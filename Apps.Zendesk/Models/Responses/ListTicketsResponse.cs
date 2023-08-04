using Apps.Zendesk.Dtos;

namespace Apps.Zendesk.Models.Responses;

public record ListTicketsResponse(TicketDto[] Tickets);