namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserDetailsChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.details_changed";

        public UserDetailsChangedHandler() : base(SubscriptionEvent) { }
    }
}