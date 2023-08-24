using Apps.Zendesk.Models.Identifiers;
using Apps.Zendesk.Models.Requests;
using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.Actions;

[ActionList]
public class TicketActions : BaseInvocable
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private ZendeskClient Client { get; }

    public TicketActions(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new ZendeskClient(invocationContext);
    }

    [Action("Get ticket", Description = "Get a specific ticket")]
    public async Task<Ticket> GetTicket([ActionParameter] TicketIdentifier ticket)
    {
        var request = new ZendeskRequest($"/api/v2/tickets/{ticket.Id}", Method.Get, Creds);
        var response = await Client.ExecuteWithHandling<SingleTicket>(request);
        return response.Ticket;
    }

    [Action("Create ticket", Description = "Create a new ticket")]
    public async Task<Ticket> CreateTicket([ActionParameter] TicketRequest input)
    {
        var request = new ZendeskRequest("/api/v2/tickets/", Method.Post, Creds);
        request.AddNewtonJson(input.Convert());
        var response = await Client.ExecuteWithHandling<SingleTicket>(request);
        return response.Ticket;
    }

    [Action("Update ticket", Description = "Update a specific ticket")]
    public async Task<Ticket> UpdateTicket([ActionParameter] TicketIdentifier ticket, [ActionParameter] TicketRequest input)
    {
        var request = new ZendeskRequest($"/api/v2/tickets/{ticket.Id}", Method.Put, Creds);
        request.AddNewtonJson(input.Convert());
        var response = await Client.ExecuteWithHandling<SingleTicket>(request);
        return response.Ticket;
    }

    [Action("Delete ticket", Description = "Delete specific ticket")]
    public Task DeleteTicket([ActionParameter] TicketIdentifier ticket)
    {
        var request = new ZendeskRequest($"/api/v2/tickets/{ticket.Id}", Method.Delete, Creds);
        return Client.ExecuteWithHandling(request);
    }
}