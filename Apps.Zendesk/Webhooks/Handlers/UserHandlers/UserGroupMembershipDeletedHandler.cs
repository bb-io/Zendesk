namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserGroupMembershipDeletedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.group_membership_deleted";

        public UserGroupMembershipDeletedHandler() : base(SubscriptionEvent) { }
    }
}