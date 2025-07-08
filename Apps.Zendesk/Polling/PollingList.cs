using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Models.Responses.Wrappers;
using Apps.Zendesk.Polling.Models;
using Apps.Zendesk.Polling.Models.Memory;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Zendesk.Polling;

[PollingEventList]
public class PollingList : BaseInvocable
{
    private ZendeskClient Client { get; }

    public PollingList(InvocationContext invocationContext)
        : base(invocationContext)
    {
        Client = new ZendeskClient(invocationContext);
    }

    [PollingEvent("On labels added to articles", "On any new labels are added to articles")]
    public async Task<PollingEventResponse<ArticleLabelsMemory, ListArticlesResponse>> OnLabelsAddedToArticles(
        PollingEventRequest<ArticleLabelsMemory> request,
        [PollingEventParameter] OnLabelsAddedInput input)
    {
        var articlesRequest =
            new ZendeskRequest($"/api/v2/help_center/articles?label_names={string.Join(',', input.Labels)}", Method.Get);
        var response = (await Client.GetPaginated<MultipleArticles>(articlesRequest))
            .SelectMany(x => x.Articles)
            .ToArray();

        var articleLabelsMap = response.ToDictionary(x => x.ContentId, x => x.Labels);

        if (request.Memory is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    ArticleLabelsMap = articleLabelsMap
                }
            };
        }

        var updatedArticles = response.Where(x => !request.Memory.ArticleLabelsMap.Keys.Contains(x.ContentId)).ToArray();

        if (!updatedArticles.Any())
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    ArticleLabelsMap = articleLabelsMap
                }
            };
        }

        return new()
        {
            FlyBird = true,
            Result = new()
            {
                Articles = updatedArticles
            },
            Memory = new()
            {
                ArticleLabelsMap = articleLabelsMap
            }
        };
    }

    [PollingEvent("On new tickets added", "On any new tickets are added")]
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