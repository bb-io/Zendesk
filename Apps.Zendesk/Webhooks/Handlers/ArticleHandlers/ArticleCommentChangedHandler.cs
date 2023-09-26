using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers;

public class ArticleCommentChangedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:article.comment_changed";

    public ArticleCommentChangedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}