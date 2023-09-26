namespace Apps.Zendesk.Webhooks.Payload;

public class WebhooksListResponse
{
    public IEnumerable<WebhookDto> Webhooks { get; set; }
}

public class WebhookDto
{
    public string Id { get; set; }

    public string Endpoint { get; set; }
}