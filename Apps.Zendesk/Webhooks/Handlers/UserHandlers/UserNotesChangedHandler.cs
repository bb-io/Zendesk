using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserNotesChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.notes_changed";

        public UserNotesChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}