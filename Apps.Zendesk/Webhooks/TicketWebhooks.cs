using Apps.Zendesk.Webhooks.Handlers.TicketHandlers;
using Apps.Zendesk.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.Zendesk.Webhooks
{
    [WebhookList("Tickets")]
    public class TicketWebhooks : BaseInvocable
    {
        private ZendeskClient Client { get; }

        public TicketWebhooks(InvocationContext invocationContext) : base(invocationContext)
        {
            Client = new ZendeskClient(invocationContext);
        }

        [Webhook("On ticket comment created", typeof(TicketCommentCreatedHandler), Description = "On ticket comment created")]
        public async Task<WebhookResponse<TicketCommentCreatedWebhook>> OnTicketCommentCreatedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<TicketCommentCreatedWebhook>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<TicketCommentCreatedWebhook>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }
    }
}
