using System.Text;
using System.Text.Json;

namespace Apps.Zendesk;

public static class WebhookLogger
{
    private const string url = "https://webhook.site/f80e4ed9-87d4-4590-8cc8-5d46f41164e2";

    public static void Log(object body)
    {
        using var client = new HttpClient();

        try
        {
            var json = JsonSerializer.Serialize(body);
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = client.Send(request);
        }
        catch (Exception ex)
        {
            var json = JsonSerializer.Serialize(ex.Message);
            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = client.Send(request);
        }
    }
}
