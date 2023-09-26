using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers;

public class ArticleVoteCreatedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:article.vote_created";

    public ArticleVoteCreatedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}