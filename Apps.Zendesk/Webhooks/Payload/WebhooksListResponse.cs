using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Payload
{
    public class WebhooksListResponse
    {
        public IEnumerable<WebhookDto> Webhooks { get; set; }
    }

    public class WebhookDto
    {
        public string Id { get; set; }

        public string Endpoint { get; set; }
    }
}
