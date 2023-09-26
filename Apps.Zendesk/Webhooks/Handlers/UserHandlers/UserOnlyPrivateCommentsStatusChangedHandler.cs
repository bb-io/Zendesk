using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserOnlyPrivateCommentsStatusChangedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.only_private_comments_changed";

    public UserOnlyPrivateCommentsStatusChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}