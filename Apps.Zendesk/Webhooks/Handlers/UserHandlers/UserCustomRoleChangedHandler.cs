using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserCustomRoleChangedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.custom_role_changed";

    public UserCustomRoleChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}