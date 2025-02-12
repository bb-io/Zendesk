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

        var articleLabelsMap = response.ToDictionary(x => x.Id, x => x.Labels);

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

        var updatedArticles = response.Where(x => !request.Memory.ArticleLabelsMap.Keys.Contains(x.Id)).ToArray();

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
}