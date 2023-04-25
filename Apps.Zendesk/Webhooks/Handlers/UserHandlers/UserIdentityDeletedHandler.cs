namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserIdentityDeletedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.identity_deleted";

        public UserIdentityDeletedHandler() : base(SubscriptionEvent) { }
    }
}