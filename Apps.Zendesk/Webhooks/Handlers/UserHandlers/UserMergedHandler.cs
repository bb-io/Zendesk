namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserMergedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type/user.merged";

        public UserMergedHandler() : base(SubscriptionEvent) { }
    }
}