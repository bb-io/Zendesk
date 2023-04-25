namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserCustomRoleChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.custom_role_changed";

        public UserCustomRoleChangedHandler() : base(SubscriptionEvent) { }
    }
}