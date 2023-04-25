namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserLastLoginChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.last_login_changed";

        public UserLastLoginChangedHandler() : base(SubscriptionEvent) { }
    }
}