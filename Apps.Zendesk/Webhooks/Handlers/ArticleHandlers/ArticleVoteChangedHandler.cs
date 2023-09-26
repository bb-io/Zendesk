using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers;

public class ArticleVoteChangedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:article.vote_changed";

    public ArticleVoteChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}