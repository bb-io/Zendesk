using Apps.Zendesk.Models.Requests;
using Apps.Zendesk.Webhooks.Handlers.TicketHandlers;
using Apps.Zendesk.Webhooks.Payload;
using Apps.Zendesk.Webhooks.Responses;
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
        public async Task<WebhookResponse<TicketCommentCreatedEventDto>> OnTicketCommentCreatedHandler(WebhookRequest webhookRequest,
            [WebhookParameter] TicketCommentFiltersInput? input)
        {
            var data = JsonConvert.DeserializeObject<TicketCommentCreatedWebhook>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }

            if (!string.IsNullOrWhiteSpace(input?.IgnoreIfBodyContains))
            {
                var body = data.Event?.Comment?.Body ?? string.Empty;
                if (body.IndexOf(input.IgnoreIfBodyContains, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return new WebhookResponse<TicketCommentCreatedEventDto>
                    {
                        HttpResponseMessage = null,
                        ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                        Result = null
                    };
                }
            }

            if (input?.OnlyPublic == true)
            {
                var isPublic = data.Event?.Comment?.IsPublic ?? false;
                if (!isPublic)
                {
                    return new WebhookResponse<TicketCommentCreatedEventDto>
                    {
                        HttpResponseMessage = null,
                        ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                        Result = null
                    };
                }
            }

            if (input?.IgnoreIfViaWebService == true)
            {
                var channel = data.Detail?.Via?.Channel;
                if (string.Equals(channel, "web_service", StringComparison.OrdinalIgnoreCase))
                {
                    return new WebhookResponse<TicketCommentCreatedEventDto>
                    {
                        HttpResponseMessage = null,
                        ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                        Result = null
                    };
                }
            }

            return new WebhookResponse<TicketCommentCreatedEventDto>
            {
                HttpResponseMessage = null,
                Result = new TicketCommentCreatedEventDto(data)
            };
        }
    }
}
