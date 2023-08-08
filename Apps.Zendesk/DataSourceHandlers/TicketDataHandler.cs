using Apps.Zendesk.Actions;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

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
        var actions = new TicketActions();
        var articles = actions.ListTickets(Creds);

        return articles.Tickets
            .Where(x => context.SearchString == null ||
                        x.Subject.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.CreatedAt)
            .Take(20)
            .ToDictionary(x => x.Id, x => x.Subject);
    }
}