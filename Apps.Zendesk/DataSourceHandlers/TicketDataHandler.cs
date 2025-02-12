using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class TicketDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public TicketDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(InvocationContext);
        IEnumerable<Ticket> tickets;
        if (string.IsNullOrEmpty(context.SearchString))
        {
            var request = new ZendeskRequest("/api/v2/tickets", Method.Get);
            var result = await client.ExecuteWithHandling<MultipleTickets>(request);
            tickets = result.Tickets;
        }
        else
        {
            var request = new ZendeskRequest("/api/v2/search", Method.Get);
            request.AddQueryParameter("query", context.SearchString);
            var result = await client.ExecuteWithHandling<SearchResponse<Ticket>>(request);
            tickets = result.Results;
        }

        return tickets
            .OrderByDescending(x => x.UpdatedAt)
            .ToDictionary(x => x.Id.ToString(), x => x.Subject);

    }
}