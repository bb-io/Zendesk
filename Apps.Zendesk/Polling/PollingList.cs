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
    public async Task<PollingEventResponse<ArticleLabelsMemory, ListArticlesResponse>> OnLabelsAddedToArticles(
        PollingEventRequest<ArticleLabelsMemory> request,
        [PollingEventParameter] OnLabelsAddedInput input)
    {
        long startTimeUnix = request.Memory is null
            ? DateTime.UtcNow.ToUnixTimeSeconds()
            : request.Memory.LastInteractionDate.ToUnixTimeSeconds();

        var endpoint = $"/api/v2/help_center/incremental/articles?start_time={startTimeUnix}";
        var articlesResponse = await Client.GetPaginatedIncremental<MultipleArticles>(
            new ZendeskRequest(endpoint, Method.Get),
            r => r.Articles?.Count() ?? 0);

        var articleMap = articlesResponse
            .SelectMany(x => x.Articles)
            .ToDictionary(a => a.ContentId);

        // Deep search: on some Zendesk instances label changes do not update updated_at,
        // so the article never re-appears in the incremental feed. Querying by label directly
        // catches those cases. Opt-in because it adds one API call per target label per run.
        if (input.DeepSearch == true)
        {
            var targetLabels = (input.Labels ?? Enumerable.Empty<string>()).ToList();
            foreach (var label in targetLabels)
            {
                var labelReq = new ZendeskRequest("/api/v2/help_center/articles", Method.Get);
                labelReq.AddQueryParameter("label_names", label);
                var labelPages = await Client.GetPaginatedIncremental<MultipleArticles>(
                    labelReq, r => r.Articles?.Count() ?? 0, pageSize: 100);
                foreach (var article in labelPages.SelectMany(p => p.Articles))
                    articleMap[article.ContentId] = article;
            }
        }

        var updatedArticles = articleMap.Values.ToArray();
        var previousLabels = request.Memory?.ArticleLabels ?? new Dictionary<string, List<string>>();
        var (articlesWithLabelAdded, newMemory) = DetectNewlyLabeledArticles(updatedArticles, previousLabels, input.Labels);

        var isFirstRun = request.Memory is null;
        return new()
        {
            FlyBird = !isFirstRun && articlesWithLabelAdded.Count > 0,
            Result = !isFirstRun && articlesWithLabelAdded.Count > 0 ? new() { Articles = articlesWithLabelAdded } : null,
            Memory = new() { LastInteractionDate = DateTime.UtcNow, ArticleLabels = newMemory }
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

    internal static (List<Article> Matches, Dictionary<string, List<string>> UpdatedMemory) DetectNewlyLabeledArticles(
        Article[] updatedArticles,
        Dictionary<string, List<string>> previousLabels,
        IEnumerable<string>? targetLabels)
    {
        var targets = (targetLabels ?? Enumerable.Empty<string>()).ToHashSet(StringComparer.OrdinalIgnoreCase);
        var matches = new List<Article>();
        var updatedMemory = new Dictionary<string, List<string>>(previousLabels);

        foreach (var article in updatedArticles)
        {
            var current = (article.Labels ?? Enumerable.Empty<string>()).ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (previousLabels.TryGetValue(article.ContentId, out var known))
            {
                var knownSet = new HashSet<string>(known, StringComparer.OrdinalIgnoreCase);
                bool newLabelAdded = targets.Count == 0
                    ? current.Except(knownSet).Any()
                    : current.Intersect(targets).Any(l => !knownSet.Contains(l));

                if (newLabelAdded)
                    matches.Add(article);
            }

            updatedMemory[article.ContentId] = current.ToList();
        }

        return (matches, updatedMemory);
    }
}
