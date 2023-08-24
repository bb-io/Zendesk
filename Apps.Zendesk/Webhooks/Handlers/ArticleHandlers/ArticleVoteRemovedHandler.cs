using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers
{
    public class ArticleVoteRemovedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:article.vote_removed";

        public ArticleVoteRemovedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}
