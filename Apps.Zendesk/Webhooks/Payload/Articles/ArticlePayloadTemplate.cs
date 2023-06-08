using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Payload.Articles
{
    public class ArticlePayloadTemplate<T>
    {
        public int AccountId { get; set; }
        public Detail Detail { get; set; }
        public T Event { get; set; }
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string ZendeskEventVersion { get; set; }
    }

    public class Detail
    {
        public string BrandId { get; set; }
        public string Id { get; set; }
    }
}
