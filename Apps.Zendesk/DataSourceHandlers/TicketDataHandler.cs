using Apps.Zendesk.Actions;
using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class TicketDataHandler : BaseInvocable, IDataSourceHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    public TicketDataHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public Dictionary<string, string> GetData(DataSourceContext context)
    {
        var client = new ZendeskClient(InvocationContext);
        IEnumerable<Ticket> tickets;
        if (string.IsNullOrEmpty(context.SearchString))
        {
            var request = new ZendeskRequest("/api/v2/tickets", Method.Get, Creds);
            var result = client.Execute<MultipleTickets>(request);
            tickets = result.Tickets;
        }
        else
        {
            var request = new ZendeskRequest("/api/v2/search", Method.Get, Creds);
            request.AddQueryParameter("query", context.SearchString);
            var result = client.Execute<SearchResponse<Ticket>>(request);
            tickets = result.Results;
        }

        return tickets
            .OrderByDescending(x => x.UpdatedAt)
            .ToDictionary(x => x.Id.ToString(), x => x.Subject);

    }
}