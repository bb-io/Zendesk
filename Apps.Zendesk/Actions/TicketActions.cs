using Apps.Zendesk.Dtos;
using Apps.Zendesk.Models.Input.Ticket;
using Apps.Zendesk.Models.Requests.Ticket;
using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Zendesk.Actions;

[ActionList]
public class TicketActions
{
    [Action("List tickets", Description = "List all tickets")]
    public ListTicketsResponse ListTickets(
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var client = new ZendeskClient(creds);
        var request = new ZendeskRequest("/api/v2/tickets", Method.Get, creds);

        var response = client.GetPaginated<TicketsResponseWrapper>(request);
        var tickets = response.SelectMany(x => x.Tickets).ToArray();

        return new(tickets);
    }

    [Action("Get ticket", Description = "Get specific ticket")]
    public TicketDto GetTicket(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] TicketIdRequest input)
    {
        var client = new ZendeskClient(creds);
        var request = new ZendeskRequest($"/api/v2/tickets/{input.TicketId}", Method.Get, creds);

        return client.Execute<TicketResponse>(request).Ticket;
    }

    [Action("Create ticket", Description = "Create a new ticket")]
    public TicketDto CreateTicket(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] CreateTicketInput input)
    {
        var client = new ZendeskClient(creds);
        var request = new ZendeskRequest("/api/v2/tickets/", Method.Post, creds);
        request.AddJsonBody(new ManageTicketRequest(input));

        return client.Execute<TicketResponse>(request).Ticket;
    }

    [Action("Update ticket", Description = "Update specific ticket")]
    public TicketDto UpdateTicket(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] TicketIdRequest ticket,
        [ActionParameter] CreateTicketInput input)
    {
        var client = new ZendeskClient(creds);
        var request = new ZendeskRequest($"/api/v2/tickets/{ticket.TicketId}", Method.Put, creds);
        request.AddJsonBody(new ManageTicketRequest(input));

        return client.Execute<TicketResponse>(request).Ticket;
    }

    [Action("Delete ticket", Description = "Delete specific ticket")]
    public Task DeleteTicket(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] TicketIdRequest input)
    {
        var client = new ZendeskClient(creds);
        var request = new ZendeskRequest($"/api/v2/tickets/{input.TicketId}", Method.Delete, creds);

        return client.ExecuteWithHandling(request);
    }
}