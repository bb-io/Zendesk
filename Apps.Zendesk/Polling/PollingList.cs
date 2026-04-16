using Apps.Zendesk.Extensions;
using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Models.Responses.Wrappers;
using Apps.Zendesk.Polling.Models;
using Apps.Zendesk.Polling.Models.Memory;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Zendesk.Polling;

[PollingEventList]
public class PollingList(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    private ZendeskClient Client { get; } = new(invocationContext);

    [PollingEvent("On labels added to articles", "On labels are added to articles")]
    public async Task<PollingEventResponse<DateMemory, ListArticlesResponse>> OnLabelsAddedToArticles(
        PollingEventRequest<DateMemory> request,
        [PollingEventParameter] OnLabelsAddedInput input)
    {
        if (request.Memory is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new() { LastInteractionDate = DateTime.UtcNow }
            };
        }

        long startTimeUnix = request.Memory.LastInteractionDate.ToUnixTimeSeconds();
        var endpoint = $"/api/v2/help_center/incremental/articles?start_time={startTimeUnix}";
        var articlesRequest = new ZendeskRequest(endpoint, Method.Get);

        var articlesResponse = await Client.GetPaginatedIncremental<MultipleArticles>(
            articlesRequest, 
            r => r.Articles?.Count() ?? 0);
        
        var updatedArticles = articlesResponse
            .SelectMany(x => x.Articles)
            .AsEnumerable()
            .WhereIntersects(input.Labels, x => x.Labels)
            .ToArray();

        if (updatedArticles.Length == 0)
        {
            return new()
            {
                FlyBird = false,
                Memory = new() { LastInteractionDate = DateTime.UtcNow }
            };
        }

        return new()
        {
            FlyBird = true,
            Result = new() { Articles = updatedArticles },
            Memory = new() { LastInteractionDate = DateTime.UtcNow }
        };
    }

    [PollingEvent("On new tickets added", "On new tickets are added")]
    public async Task<PollingEventResponse<DateMemory, ListTicketsResponse>> OnTicketsAddedToArticles(
       PollingEventRequest<DateMemory> request)
    {
        var allTickets = new List<Ticket>();
        string? next = "/api/v2/tickets?per_page=100";

        do
        {
            var page = await Client.ExecuteWithHandling<MultipleTickets>(
                new ZendeskRequest(next, Method.Get));
            allTickets.AddRange(page.Tickets);
            next = page.NextPage;
        }
        while (!string.IsNullOrEmpty(next));


        if (request.Memory == null)
        {
            var initial = allTickets.Any()
                ? allTickets.Max(t => t.CreatedAt)
                : DateTime.UtcNow;

            return new PollingEventResponse<DateMemory, ListTicketsResponse>
            {
                FlyBird = false,
                Memory = new DateMemory { LastInteractionDate = initial }
            };
        }

        var newTickets = allTickets
       .Where(t => t.CreatedAt > request.Memory.LastInteractionDate)
       .ToList();

        if (!newTickets.Any())
        {
            return new PollingEventResponse<DateMemory, ListTicketsResponse>
            {
                FlyBird = false,
                Memory = request.Memory
            };
        }

        var newest = newTickets.Max(t => t.CreatedAt);
        request.Memory.LastInteractionDate = newest;

        return new PollingEventResponse<DateMemory, ListTicketsResponse>
        {
            FlyBird = true,
            Memory = request.Memory,
            Result = new ListTicketsResponse { Tickets = newTickets }
        };
    }
}
