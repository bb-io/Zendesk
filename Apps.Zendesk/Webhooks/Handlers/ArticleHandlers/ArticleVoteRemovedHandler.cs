using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Handlers.ArticleHandlers
{
    public class ArticleVoteRemovedHandler : BaseWebhookHandler
    {
        const string SubscriptionEvent = "zen:event-type:article.vote_removed";

        public ArticleVoteRemovedHandler() : base(SubscriptionEvent) { }
    }
}
