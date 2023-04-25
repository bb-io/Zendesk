namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserSuspendedStatusChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.suspended_changed";

        public UserSuspendedStatusChangedHandler() : base(SubscriptionEvent) { }
    }
}