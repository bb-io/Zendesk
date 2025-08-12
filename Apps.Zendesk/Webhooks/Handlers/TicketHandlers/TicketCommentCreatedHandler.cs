using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.TicketHandlers
{
    internal class TicketCommentCreatedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:ticket.comment_added";

        public TicketCommentCreatedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}