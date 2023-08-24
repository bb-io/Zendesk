using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserDefaultGroupChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.default_group_changed";

        public UserDefaultGroupChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}