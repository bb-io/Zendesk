using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers;

public class ArticleCommentCreatedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:article.comment_created";

    public ArticleCommentCreatedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}