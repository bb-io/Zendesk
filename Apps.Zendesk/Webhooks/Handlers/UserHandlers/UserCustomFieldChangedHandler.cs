using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserCustomFieldChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.custom_field_changed";

        public UserCustomFieldChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}