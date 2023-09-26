using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserTimeZoneChangedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.time_zone_changed";

    public UserTimeZoneChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}