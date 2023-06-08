using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Payload.Articles
{
    public class CommentUnpublishedEvent
    {
        public CommentPublished Comment { get; set; }
    }
}
