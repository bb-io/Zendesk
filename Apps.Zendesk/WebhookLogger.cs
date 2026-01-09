using System.Text;
using Newtonsoft.Json;

namespace Apps.Zendesk;

public static class WebhookLogger
{
    private static readonly HttpClient client = new HttpClient();

    public static void Log(string url, object body)
    {
        try
        {
            var json = JsonConvert.SerializeObject(body);

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            client.Send(request);
        }
        catch (Exception ex)
        {
            var json = JsonConvert.SerializeObject(ex.Message);

            using var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            client.Send(request);
        }
    }
}