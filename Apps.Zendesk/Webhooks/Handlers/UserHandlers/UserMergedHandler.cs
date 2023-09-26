using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserMergedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type/user.merged";

    public UserMergedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}