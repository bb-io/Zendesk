namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers
{
    public class ArticleAuthorChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:article.author_changed";

        public ArticleAuthorChangedHandler() : base(SubscriptionEvent) { }
    }
}
