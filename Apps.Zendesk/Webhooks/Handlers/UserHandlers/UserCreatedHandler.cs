namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserCreatedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.created";

        public UserCreatedHandler() : base(SubscriptionEvent) { }
    }
}