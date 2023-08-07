namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserGroupMembershipCreatedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.group_membership_created";

        public UserGroupMembershipCreatedHandler() : base(SubscriptionEvent) { }
    }
}