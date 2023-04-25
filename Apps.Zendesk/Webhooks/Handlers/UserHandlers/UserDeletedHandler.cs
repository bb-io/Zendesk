namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserDeletedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.deleted";

        public UserDeletedHandler() : base(SubscriptionEvent) { }
    }
}