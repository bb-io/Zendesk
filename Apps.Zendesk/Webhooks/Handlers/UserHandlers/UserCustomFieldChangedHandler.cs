namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserCustomFieldChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.custom_field_changed";

        public UserCustomFieldChangedHandler() : base(SubscriptionEvent) { }
    }
}