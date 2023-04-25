using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Payload
{
    public class GroupEvent
    {
        public Group Group { get; set; }
    }

    public class Group
    {
        public string Id { get; set; }
    }
}
