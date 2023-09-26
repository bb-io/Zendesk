using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserAliasChangedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.alias_changed";

    public UserAliasChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}