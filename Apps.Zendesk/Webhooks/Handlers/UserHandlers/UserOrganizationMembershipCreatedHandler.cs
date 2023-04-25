namespace Apps.Zendesk.Webhooks.Handlers
{
    public class UserOrganizationMembershipCreatedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.organization_membership_created";

        public UserOrganizationMembershipCreatedHandler() : base(SubscriptionEvent) { }
    }
}