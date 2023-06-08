using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Payload.Articles
{
    public class CommentPublishedEvent
    {
        public CommentPublished Comment { get; set; }
    }

    public class CommentPublished
    {
        public string Id { get; set; }
        public string Locale { get; set; }
    }
}
