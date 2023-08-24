using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserIdentityChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type/user.identity_changed";

        public UserIdentityChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}