using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Payload
{
    public class SimpleStringEvent
    {
        public string Current { get; set; }
        public string Previous { get; set; }
    }
}
