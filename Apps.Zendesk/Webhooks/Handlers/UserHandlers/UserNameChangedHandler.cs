namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserNameChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.name_changed";

        public UserNameChangedHandler() : base(SubscriptionEvent) { }
    }
}