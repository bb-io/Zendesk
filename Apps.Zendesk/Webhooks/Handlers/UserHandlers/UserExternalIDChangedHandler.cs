using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserExternalIDChangedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.external_id_changed";

    public UserExternalIDChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}