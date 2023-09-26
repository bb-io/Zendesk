using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserDetailsChangedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.details_changed";

    public UserDetailsChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}