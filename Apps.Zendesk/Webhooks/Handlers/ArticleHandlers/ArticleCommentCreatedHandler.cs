namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers
{
    public class ArticleCommentCreatedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:article.comment_created";

        public ArticleCommentCreatedHandler() : base(SubscriptionEvent) { }
    }
}
