namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserOrganizationMembershipDeletedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.organization_membership_deleted";

        public UserOrganizationMembershipDeletedHandler() : base(SubscriptionEvent) { }
    }
}