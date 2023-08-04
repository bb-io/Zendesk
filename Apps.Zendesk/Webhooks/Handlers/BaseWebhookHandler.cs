using Apps.Zendesk.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;

namespace Apps.Zendesk.Webhooks.Handlers
{
    public class BaseWebhookHandler : IWebhookEventHandler
    {

        private string SubscriptionEvent;

        public BaseWebhookHandler(string subEvent)
        {
            SubscriptionEvent = subEvent;
        }

        public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
        {
            var client = new ZendeskClient(authenticationCredentialsProvider);
            var request = new ZendeskRequest($"/api/v2/webhooks", Method.Post, authenticationCredentialsProvider);
            request.AddJsonBody(new
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
            await client.ExecuteAsync(request);
        }

        public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProvider, Dictionary<string, string> values)
        {
            var client = new ZendeskClient(authenticationCredentialsProvider);
            var getRequest = new ZendeskRequest($"/api/v2/webhooks?filter[name_contains]={SubscriptionEvent}", Method.Get, authenticationCredentialsProvider);
            var webhooks = await client.GetAsync<WebhooksListResponse>(getRequest);
            var webhookId = webhooks.Webhooks.First().Id;

            var deleteRequest = new ZendeskRequest($"/api/v2/webhooks/{webhookId}", Method.Delete, authenticationCredentialsProvider);
            await client.ExecuteAsync(deleteRequest);
        }
    }
}
