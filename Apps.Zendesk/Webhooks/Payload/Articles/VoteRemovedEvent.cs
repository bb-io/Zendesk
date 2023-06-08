using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Payload.Articles
{
    public class VoteRemovedEvent
    {
        public VoteRemoved Vote { get; set; }
    }

    public class VoteRemoved
    {
        public string Id { get; set; }
    }
}
