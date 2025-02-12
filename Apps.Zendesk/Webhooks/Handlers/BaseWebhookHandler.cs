using Apps.Zendesk.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.Zendesk.Webhooks.Handlers;

public class BaseWebhookHandler(InvocationContext invocationContext, string subEvent)
    : BaseInvocable(invocationContext), IWebhookEventHandler
{
    private ZendeskClient Client { get; } = new(invocationContext);

    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var request = new ZendeskRequest($"/api/v2/webhooks", Method.Post);
        request.AddNewtonJson(new
        {
            webhook = new
            {
                name = subEvent,
                description = $"Bird ID: {InvocationContext.Bird?.Id}, Bird name: {InvocationContext.Bird?.Name}",
                endpoint = values["payloadUrl"],
                status = "active",
                http_method = "POST",
                request_format = "json",
                subscriptions = new[]
                {
                    subEvent
                }
            }
        });
        await Client.ExecuteAsync(request);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var getRequest = new ZendeskRequest($"/api/v2/webhooks?filter[name_contains]={subEvent}", Method.Get);
        var webhooks = await Client.GetAsync<WebhooksListResponse>(getRequest);
        var weebhook = webhooks!.Webhooks.FirstOrDefault(x => x.Endpoint == values["payloadUrl"])
            ?? throw new Exception("There is no webhook with the specified endpoint that matches the payload url");

        var deleteRequest = new ZendeskRequest($"/api/v2/webhooks/{weebhook.Id}", Method.Delete);
        await Client.ExecuteAsync(deleteRequest);
    }
}