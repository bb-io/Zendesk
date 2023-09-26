using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserTagsChangedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.tags_changed";

    public UserTagsChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}