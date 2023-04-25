namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserPhotoChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.photo_changed";

        public UserPhotoChangedHandler() : base(SubscriptionEvent) { }
    }
}