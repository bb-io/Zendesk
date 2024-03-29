﻿using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers;

public class ArticleUnpublishedHandler : BaseWebhookHandler
{
    const string SubscriptionEvent = "zen:event-type:article.unpublished";

    public ArticleUnpublishedHandler(InvocationContext invocationContext) : base(invocationContext, SubscriptionEvent) { }
}