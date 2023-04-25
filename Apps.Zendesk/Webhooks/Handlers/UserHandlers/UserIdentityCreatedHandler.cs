namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserIdentityCreatedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type/user.identity_created";

        public UserIdentityCreatedHandler() : base(SubscriptionEvent) { }
    }
}