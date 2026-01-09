using System.Text;
using System.Text.Json;

namespace Apps.Zendesk;

public static class WebhookLogger
{
    public static void Log(string url, object body)
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
