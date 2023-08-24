using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserPasswordChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.password_changed";

        public UserPasswordChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}