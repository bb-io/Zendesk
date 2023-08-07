namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserIdentityChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type/user.identity_changed";

        public UserIdentityChangedHandler() : base(SubscriptionEvent) { }
    }
}