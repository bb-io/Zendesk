using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers;

public class ArticleCommentUnpublishedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:article.comment_unpublished";

    public ArticleCommentUnpublishedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}