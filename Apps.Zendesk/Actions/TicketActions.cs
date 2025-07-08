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

[ActionList("Tickets")]
public class TicketActions : BaseInvocable
{
    private ZendeskClient Client { get; }

    public TicketActions(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new ZendeskClient(invocationContext);
    }

    [Action("Search tickets", Description = "Search tickets")]
    public async Task<ListTicketsResponse> SearchTickets([ActionParameter] SearchTicketsRequest input)
    {
        bool useSearchApi =
         !string.IsNullOrWhiteSpace(input.Status) ||
         !string.IsNullOrWhiteSpace(input.Priority);

        List<Ticket> tickets;

        if (useSearchApi)
        {
            var terms = new List<string> { "type:ticket" };
            if (!string.IsNullOrWhiteSpace(input.Status))
                terms.Add($"status:{input.Status}");
            if (!string.IsNullOrWhiteSpace(input.Priority))
                terms.Add($"priority:{input.Priority}");

            var request = new ZendeskRequest($"/api/v2/search", Method.Get);
            request.AddQueryParameter("query", string.Join(" ", terms));

            tickets = await Client.GetPaginatedResults<Ticket>(request);
        }
        else
        {
            var all = new List<Ticket>();
            string? next = "/api/v2/tickets?per_page=100";

            do
            {
                var page = await Client.ExecuteWithHandling<MultipleTickets>(
                    new ZendeskRequest(next, Method.Get));
                all.AddRange(page.Tickets);
                next = page.NextPage;
            }
            while (!string.IsNullOrEmpty(next));

            tickets = all;
        }

        return new ListTicketsResponse { Tickets = tickets };
    }


    [Action("Get ticket", Description = "Get a specific ticket")]
    public async Task<Ticket> GetTicket([ActionParameter] TicketIdentifier ticket)
    {
        var request = new ZendeskRequest($"/api/v2/tickets/{ticket.Id}", Method.Get);
        var response = await Client.ExecuteWithHandling<SingleTicket>(request);
        return response.Ticket;
    }

    [Action("Create ticket", Description = "Create a new ticket")]
    public async Task<Ticket> CreateTicket([ActionParameter] TicketRequest input)
    {
        var request = new ZendeskRequest("/api/v2/tickets/", Method.Post);
        request.AddNewtonJson(input.Convert());
        var response = await Client.ExecuteWithHandling<SingleTicket>(request);
        return response.Ticket;
    }

    [Action("Update ticket", Description = "Update a specific ticket")]
    public async Task<Ticket> UpdateTicket([ActionParameter] TicketIdentifier ticket, [ActionParameter] TicketRequest input)
    {
        var request = new ZendeskRequest($"/api/v2/tickets/{ticket.Id}", Method.Put);
        request.AddNewtonJson(input.Convert());
        var response = await Client.ExecuteWithHandling<SingleTicket>(request);
        return response.Ticket;
    }

    [Action("Delete ticket", Description = "Delete specific ticket")]
    public Task DeleteTicket([ActionParameter] TicketIdentifier ticket)
    {
        var request = new ZendeskRequest($"/api/v2/tickets/{ticket.Id}", Method.Delete);
        return Client.ExecuteWithHandling(request);
    }
}