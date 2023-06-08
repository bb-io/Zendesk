using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Payload.Articles
{
    public class CommentCreatedEvent
    {
        public Comment Comment { get; set; }
    }

    public class Comment
    {
        public string AuthorId { get; set; }
        public string Id { get; set; }
        public string Locale { get; set; }
    }
}
