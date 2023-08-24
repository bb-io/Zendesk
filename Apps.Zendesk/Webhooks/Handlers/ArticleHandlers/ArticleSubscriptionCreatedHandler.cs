using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers
{
    public class ArticleSubscriptionCreatedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:article.subscription_created";

        public ArticleSubscriptionCreatedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
    }
}