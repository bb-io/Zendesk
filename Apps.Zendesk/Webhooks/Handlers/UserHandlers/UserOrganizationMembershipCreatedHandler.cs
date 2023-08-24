using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers
{
    public class UserOrganizationMembershipCreatedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:user.organization_membership_created";

        public UserOrganizationMembershipCreatedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}