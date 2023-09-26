using Apps.Zendesk.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.Zendesk.Webhooks.Handlers;

public class BaseWebhookHandler : BaseInvocable, IWebhookEventHandler
{
    private IEnumerable<AuthenticationCredentialsProvider> Creds =>
        InvocationContext.AuthenticationCredentialsProviders;

    private string SubscriptionEvent;
    private ZendeskClient Client { get; }

    public BaseWebhookHandler(InvocationContext invocationContext, string subEvent) : base(invocationContext)
    {
        SubscriptionEvent = subEvent;
        Client = new ZendeskClient(invocationContext);
    }

    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var request = new ZendeskRequest($"/api/v2/webhooks", Method.Post, Creds);
        request.AddNewtonJson(new
        {
            webhook = new
            {
                name = SubscriptionEvent,
                description = "",
                endpoint = values["payloadUrl"],
                status = "active",
                http_method = "POST",
                request_format = "json",
                subscriptions = new[]
                {
                    SubscriptionEvent
                }
            }
        });
        await Client.ExecuteAsync(request);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
    {
        var getRequest = new ZendeskRequest($"/api/v2/webhooks?filter[name_contains]={SubscriptionEvent}", Method.Get, Creds);
        var webhooks = await Client.GetAsync<WebhooksListResponse>(getRequest);
        var webhookId = webhooks.Webhooks.First().Id;

        var deleteRequest = new ZendeskRequest($"/api/v2/webhooks/{webhookId}", Method.Delete, Creds);
        await Client.ExecuteAsync(deleteRequest);
    }
}