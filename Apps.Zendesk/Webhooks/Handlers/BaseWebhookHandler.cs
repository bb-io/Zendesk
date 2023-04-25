using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            var authHeader = authenticationCredentialsProvider.First(p => p.KeyName == "Authorization").Value;
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
            var authHeader = authenticationCredentialsProvider.First(p => p.KeyName == "Authorization").Value;
            var client = new ZendeskClient(authenticationCredentialsProvider);
            var getRequest = new ZendeskRequest($"/api2/v2/webhooks?name={SubscriptionEvent}", Method.Get, authenticationCredentialsProvider);

        }
    }
}
