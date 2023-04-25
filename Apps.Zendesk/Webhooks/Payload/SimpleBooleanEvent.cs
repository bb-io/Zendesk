using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Payload
{
    public class SimpleBooleanEvent
    {
        public bool Current { get; set; }
        public bool Previous { get; set; }
    }
}
