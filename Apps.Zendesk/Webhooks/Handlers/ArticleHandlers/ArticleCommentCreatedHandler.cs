using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers
{
    public class ArticleCommentCreatedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:article.comment_created";

        public ArticleCommentCreatedHandler() : base(SubscriptionEvent) { }
    }
}
