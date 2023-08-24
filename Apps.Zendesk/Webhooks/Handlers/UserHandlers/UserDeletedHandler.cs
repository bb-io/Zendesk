using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserDeletedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.deleted";

        public UserDeletedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}