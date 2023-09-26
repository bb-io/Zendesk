using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserSuspendedStatusChangedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.suspended_changed";

    public UserSuspendedStatusChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}