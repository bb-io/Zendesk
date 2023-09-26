using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserCreatedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.created";

    public UserCreatedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}