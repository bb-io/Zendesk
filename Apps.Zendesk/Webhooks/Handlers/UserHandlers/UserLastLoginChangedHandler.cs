namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserLastLoginChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.last_login_changed";

        public UserLastLoginChangedHandler() : base(SubscriptionEvent) { }
    }
}