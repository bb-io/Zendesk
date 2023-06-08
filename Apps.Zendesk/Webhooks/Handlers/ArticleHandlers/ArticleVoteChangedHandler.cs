using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers
{
    public class ArticleVoteChangedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:article.vote_changed";

        public ArticleVoteChangedHandler() : base(SubscriptionEvent) { }
    }
}
