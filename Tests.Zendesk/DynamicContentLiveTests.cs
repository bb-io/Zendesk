using Apps.Zendesk;
using Apps.Zendesk.Actions;
using Apps.Zendesk.Models;
using Apps.Zendesk.Polling;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;
using System.Text;

namespace ZendeskTests;

[TestClass]
public class DynamicContentLiveTests
{
    [TestMethod]
    public async Task EnglishRoundtripAndPolling()
    {
        var endpoint = Environment.GetEnvironmentVariable("ZENDESK_TEST_URL");
        var token = Environment.GetEnvironmentVariable("ZENDESK_TEST_TOKEN");
        var email = Environment.GetEnvironmentVariable("ZENDESK_TEST_EMAIL");
        if (string.IsNullOrWhiteSpace(endpoint) || string.IsNullOrWhiteSpace(token)) Assert.Inconclusive("Set ZENDESK_TEST_URL and ZENDESK_TEST_TOKEN (and ZENDESK_TEST_EMAIL for API-token authentication) to run this live test.");
        var credentials = new List<AuthenticationCredentialsProvider> { new("api_endpoint", endpoint!), new("access_token", token!) };
        if (!string.IsNullOrWhiteSpace(email)) { credentials.Add(new("ConnectionType", "API token")); credentials.Add(new("email", email)); }
        var context = new InvocationContext { AuthenticationCredentialsProviders = credentials };
        var client = new ZendeskClient(context); var api = new DynamicContentApi(client);
        var locale = (await api.Locales()).FirstOrDefault(x => x.Code.Equals("en-US", StringComparison.OrdinalIgnoreCase));
        if (locale == null) Assert.Inconclusive("The account has no enabled en-US Support locale.");
        var name = "blackbird-test-" + Guid.NewGuid().ToString("N");
        var create = new ZendeskRequest("/api/v2/dynamic_content/items", Method.Post);
        create.AddNewtonJson(new { item = new { name, default_locale_id = locale!.Id, variants = new[] { new { locale_id = locale.Id, content = "Original test text", active = true, @default = true } } } });
        var item = (await client.ExecuteWithHandling<SingleDynamicItem>(create)).Item;
        try
        {
            var files = new LiveFiles(); var actions = new DynamicContentActions(context, files); var poll = new DynamicContentPolling(context);
            var baseline = await poll.Poll(new PollingEventRequest<DynamicPollingMemory>(), new() { Name = name });
            Assert.IsFalse(baseline.FlyBird);
            var download = await actions.Download(new() { Id = item.Id }, new());
            files.Bytes = Encoding.UTF8.GetBytes(Encoding.UTF8.GetString(files.Bytes).Replace("Original test text", "Revised test text"));
            var uploaded = await actions.Upload(new() { Content = download.Content, Language = locale.Code });
            Assert.AreEqual("updated", uploaded.Operation);
            var readBack = await api.Get(item.Id); Assert.AreEqual("Revised test text", readBack!.Variants.Single(x => x.LocaleId == locale.Id).Content);
            var changed = await poll.Poll(new PollingEventRequest<DynamicPollingMemory> { Memory = baseline.Memory }, new() { Name = name });
            Assert.AreEqual("updated", changed.Result!.Single().EventType);
            var repeat = await poll.Poll(new PollingEventRequest<DynamicPollingMemory> { Memory = changed.Memory }, new() { Name = name }); Assert.IsFalse(repeat.FlyBird);
        }
        finally { await client.ExecuteWithHandling(new ZendeskRequest($"/api/v2/dynamic_content/items/{item.Id}", Method.Delete)); }
    }
    private class LiveFiles : IFileManagementClient
    {
        public byte[] Bytes { get; set; } = Array.Empty<byte>();
        public Task<Stream> DownloadAsync(FileReference reference) => Task.FromResult<Stream>(new MemoryStream(Bytes));
        public async Task<FileReference> UploadAsync(Stream stream, string contentType, string fileName) { using var buffer = new MemoryStream(); await stream.CopyToAsync(buffer); Bytes = buffer.ToArray(); return new() { Name = fileName, ContentType = contentType }; }
    }
}
