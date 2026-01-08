using System.Text;
using System.Text.Json;

namespace Apps.Zendesk;

public static class WebhookLogger
{
    public static void Log(string url, object body)
    {
        using var client = new HttpClient();

        var json = JsonSerializer.Serialize(body);
        using var request = new HttpRequestMessage(HttpMethod.Post, url);
        request.Content = new StringContent(json, Encoding.UTF8, "application/json");

        using var response = client.Send(request);
    }
}
