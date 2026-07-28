using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.Zendesk.Utils;

public static class WebhookResponses
{
    public static WebhookResponse<T> NoFlight<T>() where T : class => new()
    {
        HttpResponseMessage = null,
        ReceivedWebhookRequestType = WebhookRequestType.Preflight,
        Result = null
    };
}