namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserCustomFieldChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.custom_field_changed";

        public UserCustomFieldChangedHandler() : base(SubscriptionEvent) { }
    }
}