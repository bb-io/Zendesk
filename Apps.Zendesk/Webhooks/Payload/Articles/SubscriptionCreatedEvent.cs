using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Payload.Articles
{
    public class SubscriptionCreatedEvent
    {
        public Subscription Subscription { get; set; }
    }

    public class Subscription
    {
        public string Id { get; set; }
        public string UserId { get; set; }
    }
}
