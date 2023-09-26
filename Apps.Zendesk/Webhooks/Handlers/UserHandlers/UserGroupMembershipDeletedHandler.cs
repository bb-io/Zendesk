using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserGroupMembershipDeletedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.group_membership_deleted";

    public UserGroupMembershipDeletedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}