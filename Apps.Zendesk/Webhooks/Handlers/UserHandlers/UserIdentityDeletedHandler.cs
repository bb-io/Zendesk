using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.UserHandlers;

public class UserIdentityDeletedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:user.identity_deleted";

    public UserIdentityDeletedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}