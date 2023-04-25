namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserTagsChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.tags_changed";

        public UserTagsChangedHandler() : base(SubscriptionEvent) { }
    }
}