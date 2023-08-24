using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserOrganizationMembershipDeletedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.organization_membership_deleted";

        public UserOrganizationMembershipDeletedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}