namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserTimeZoneChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.time_zone_changed";

        public UserTimeZoneChangedHandler() : base(SubscriptionEvent) { }
    }
}