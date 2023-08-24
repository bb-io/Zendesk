using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers
{
    public class ArticlePublishedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:article.published";

        public ArticlePublishedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}
