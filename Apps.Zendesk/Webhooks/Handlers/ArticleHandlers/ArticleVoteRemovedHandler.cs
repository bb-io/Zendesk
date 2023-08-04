namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers
{
    public class ArticleVoteRemovedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:article.vote_removed";

        public ArticleVoteRemovedHandler() : base(SubscriptionEvent) { }
    }
}
