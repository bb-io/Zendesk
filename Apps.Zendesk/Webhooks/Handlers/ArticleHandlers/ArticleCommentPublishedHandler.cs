using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers
{
    public class ArticleCommentPublishedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:article.comment_published";

        public ArticleCommentPublishedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}
