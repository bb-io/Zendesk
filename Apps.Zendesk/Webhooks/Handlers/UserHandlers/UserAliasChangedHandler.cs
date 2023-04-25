namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserAliasChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.alias_changed";

        public UserAliasChangedHandler() : base(SubscriptionEvent) { }
    }
}