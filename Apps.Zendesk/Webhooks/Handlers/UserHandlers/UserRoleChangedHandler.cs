namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserRoleChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.role_changed";

        public UserRoleChangedHandler() : base(SubscriptionEvent) { }
    }
}