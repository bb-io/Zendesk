using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Requests
{
    public class NotifySubscribersRequest
    {
        [Display("Notify subscribers?")]
        [JsonProperty("notify_subscribers")]
        public bool? NotifySubscribers { get; set; }
    }
}
