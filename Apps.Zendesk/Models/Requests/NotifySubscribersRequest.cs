using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Requests;

public class NotifySubscribersRequest
{
    [Display("Notify subscribers?")]
    [JsonProperty("notify_subscribers")]
    public bool? NotifySubscribers { get; set; }
}