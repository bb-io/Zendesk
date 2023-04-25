namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserActiveStatusChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.active_changed";

        public UserActiveStatusChangedHandler() : base(SubscriptionEvent) { }
    }
}