namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserOnlyPrivateCommentsStatusChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.only_private_comments_changed";

        public UserOnlyPrivateCommentsStatusChangedHandler() : base(SubscriptionEvent) { }
    }
}