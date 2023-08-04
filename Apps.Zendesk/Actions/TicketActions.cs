using Apps.Zendesk.Dtos;
using Apps.Zendesk.Models.Requests;
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
        var request = new ZendeskRequest("/api/v2/tickets",
            Method.Get, creds);
        
        var response = client.GetPaginated<TicketsResponseWrapper>(request);
        var tickets = response.SelectMany(x => x.Tickets).ToArray();

        return new(tickets);
    }
    
    [Action("Get ticket", Description = "Get specific ticket")]
    public TicketDto GetTicket(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Ticket ID")] string ticketId)
    {
        var client = new ZendeskClient(creds);
        var request = new ZendeskRequest($"/api/v2/tickets/{ticketId}",
            Method.Get, creds);
        
        return client.Execute<TicketResponse>(request).Ticket;
    }    
    
    [Action("Create ticket", Description = "Create a new ticket")]
    public TicketDto CreateTicket(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] CreateTicketRequest input)
    {
        var client = new ZendeskClient(creds);
        var request = new ZendeskRequest($"/api/v2/tickets/",
            Method.Post, creds);
        request.AddJsonBody(input);
        
        return client.Execute<TicketResponse>(request).Ticket;
    }
    
    [Action("Delete ticket", Description = "Delete specific ticket")]
    public Task DeleteTicket(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Ticket ID")] string ticketId)
    {
        var client = new ZendeskClient(creds);
        var request = new ZendeskRequest($"/api/v2/tickets/{ticketId}",
            Method.Delete, creds);
        
        return client.ExecuteAsync(request);
    }    
}