using Apps.Zendesk.Actions;
using Apps.Zendesk.Models;
using Apps.Zendesk.Polling;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Filters.Transformations;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json.Linq;

namespace ZendeskTests;

[TestClass]
public class DynamicContentApiTests
{
    [TestMethod]
    public async Task SearchFindAndPaginationRespectHasMore()
    {
        using var server = new FixtureServer(); var actions = server.Actions();
        var all = await actions.Search(new()); Assert.AreEqual(2, all.TotalCount);
        Assert.AreEqual(1, (await actions.Search(new() { Query = "DC.SECOND" })).TotalCount);
        Assert.AreEqual(0, (await actions.Search(new() { Query = "missing" })).TotalCount);
        Assert.AreEqual("2", (await actions.Find(new() { Placeholder = "dc.second" })).Item!.Id);
        Assert.IsTrue((await actions.Find(new() { Id = "1" })).Found);
        Assert.IsFalse((await actions.Find(new() { Id = "999" })).Found);
        var invalidEndpoint = await Assert.ThrowsExceptionAsync<Blackbird.Applications.Sdk.Common.Exceptions.PluginApplicationException>(() => actions.Find(new() { Id = "997" }));
        StringAssert.Contains(invalidEndpoint.Message, "This feature is not allowed for your Zendesk instance");
        await Assert.ThrowsExceptionAsync<Blackbird.Applications.Sdk.Common.Exceptions.PluginApplicationException>(() => actions.Find(new() { Id = "998" }));
        await Assert.ThrowsExceptionAsync<Blackbird.Applications.Sdk.Common.Exceptions.PluginMisconfigurationException>(() => actions.Find(new() { Id = "invalid" }));
        Assert.AreEqual(0, server.ExtraPageRequests);
        await Assert.ThrowsExceptionAsync<Blackbird.Applications.Sdk.Common.Exceptions.PluginMisconfigurationException>(() => actions.Find(new()));
    }
    [TestMethod]
    public async Task UploadCreatesUpdatesAndSkipsUnchangedWithTargetMetadata()
    {
        using var server = new FixtureServer(); var files = new MemoryFiles(); var actions = server.Actions(files);
        var download = await actions.Download(new() { Id = "1" }, new());
        await Assert.ThrowsExceptionAsync<Blackbird.Applications.Sdk.Common.Exceptions.PluginMisconfigurationException>(() => actions.Upload(new() { Content = download.Content }));
        var input = new DynamicUploadInput { Content = download.Content, Language = "de" };
        var created = await actions.Upload(input);
        Assert.AreEqual("created", created.Operation); Assert.AreEqual(1, server.Writes);
        var savedHtml = Encoding.UTF8.GetString(files.Bytes);
        using var stream = new MemoryStream(files.Bytes); var target = Transformation.Load(stream, "result.html", "text/html");
        Assert.AreEqual("de", target.Value!.SourceLanguage);
        StringAssert.Contains(savedHtml, "dynamic-content:1");
        var unchanged = await actions.Upload(input); Assert.AreEqual("unchanged", unchanged.Operation); Assert.AreEqual(1, server.Writes);
        input.Active = false; var updated = await actions.Upload(input); Assert.AreEqual("updated", updated.Operation); Assert.AreEqual(2, server.Writes);
        input.Language = "fr";
        await Assert.ThrowsExceptionAsync<Blackbird.Applications.Sdk.Common.Exceptions.PluginMisconfigurationException>(() => actions.Upload(input));
    }
    [TestMethod]
    public async Task XliffUploadUsesTargetLanguageAndPreservesBilingualFile()
    {
        using var server = new FixtureServer(); var files = new MemoryFiles(); var actions = server.Actions(files);
        var download = await actions.Download(new() { Id = "1" }, new());
        using var html = new MemoryStream(files.Bytes);
        var transformation = Transformation.Load(html, download.Content.Name, "text/html").Value!;
        transformation.TargetLanguage = "de";
        foreach (var segment in transformation.GetUnits().SelectMany(x => x.Segments)) segment.SetTarget("Hallo");
        using var xliff = transformation.ToStream(); using var buffer = new MemoryStream(); await xliff.CopyToAsync(buffer);
        files.Set(buffer.ToArray());
        var output = await actions.Upload(new() { Content = new FileReference { Name = "dynamic.xlf", ContentType = "application/x-xliff+xml" } });
        Assert.AreEqual("created", output.Operation); Assert.AreEqual("de", output.Language);
        using var returned = new MemoryStream(files.Bytes); var loaded = Transformation.Load(returned, output.Content.Name, output.Content.ContentType);
        Assert.IsTrue(loaded.WasBilingual); Assert.AreEqual("de", loaded.Value!.TargetLanguage);
        Assert.AreEqual("127.0.0.1:dynamic-content:1", loaded.Value.TargetSystemReference.ContentId);
        var translated = loaded.Target(); Assert.IsTrue(translated.Success, translated.Error);
        using var targetStream = translated.Value.ToStream(); using var reader = new StreamReader(targetStream);
        Assert.AreEqual("Hallo", DynamicContentFile.Extract(await reader.ReadToEndAsync()).Content);
    }
    [TestMethod]
    public async Task FailedPageDoesNotReturnPollingMemory()
    {
        using var server = new FixtureServer { FailSecondPage = true };
        var poll = new DynamicContentPolling(server.Context());
        await Assert.ThrowsExceptionAsync<Blackbird.Applications.Sdk.Common.Exceptions.PluginApplicationException>(() => poll.Poll(new(), new()));
    }

    private sealed class MemoryFiles : IFileManagementClient
    {
        public byte[] Bytes { get; private set; } = Array.Empty<byte>();
        public void Set(byte[] bytes) => Bytes = bytes;
        public Task<Stream> DownloadAsync(FileReference reference) => Task.FromResult<Stream>(new MemoryStream(Bytes));
        public async Task<FileReference> UploadAsync(Stream stream, string contentType, string fileName) { using var buffer = new MemoryStream(); await stream.CopyToAsync(buffer); Bytes = buffer.ToArray(); return new() { Name = fileName, ContentType = contentType }; }
    }
    private sealed class FixtureServer : IDisposable
    {
        private readonly HttpListener listener = new(); private readonly Task worker;
        public string Url { get; }
        public bool FailSecondPage { get; set; }
        public int ExtraPageRequests { get; private set; }
        public int Writes { get; private set; }
        private JObject? german;
        public FixtureServer()
        {
            var socket = new TcpListener(IPAddress.Loopback, 0); socket.Start(); int port = ((IPEndPoint)socket.LocalEndpoint).Port; socket.Stop();
            Url = $"http://127.0.0.1:{port}"; listener.Prefixes.Add(Url + "/"); listener.Start();
            worker = Task.Run(async () => { while (listener.IsListening) { HttpListenerContext c; try { c = await listener.GetContextAsync(); } catch (HttpListenerException) { break; } catch (ObjectDisposedException) { break; } await Handle(c); } });
        }
        public InvocationContext Context() => new() { AuthenticationCredentialsProviders = new[] { new AuthenticationCredentialsProvider("api_endpoint", Url), new AuthenticationCredentialsProvider("access_token", "fixture") } };
        public DynamicContentActions Actions(IFileManagementClient? files = null) => new(Context(), files ?? new MemoryFiles());
        private JObject Item(int id)
        {
            var variants = new JArray(new JObject { ["id"] = 10, ["locale_id"] = 1, ["content"] = "Hello", ["default"] = true, ["active"] = true });
            if (id == 1 && german != null) variants.Add(german.DeepClone());
            return new JObject { ["id"] = id, ["name"] = id == 1 ? "First" : "Second", ["placeholder"] = id == 1 ? "{{dc.first}}" : "{{dc.second}}", ["default_locale_id"] = 1, ["variants"] = variants };
        }
        private async Task Handle(HttpListenerContext c)
        {
            string path = c.Request.Url!.AbsolutePath; var response = new JObject(); int status = 200;
            if (path == "/api/v2/locales") response["locales"] = new JArray(new JObject { ["id"] = 1, ["locale"] = "en-US", ["name"] = "English" }, new JObject { ["id"] = 2, ["locale"] = "de", ["name"] = "German" });
            else if (path == "/api/v2/dynamic_content/items")
            {
                var page = c.Request.QueryString["page[after]"];
                if (page == "extra") ExtraPageRequests++;
                if (page == "second" && FailSecondPage) { status = 403; response["error"] = "Forbidden"; }
                else { response["items"] = new JArray(Item(page == null ? 1 : 2)); response["meta"] = new JObject { ["has_more"] = page == null }; response["links"] = new JObject { ["next"] = Url + (page == null ? "/api/v2/dynamic_content/items?page[after]=second" : "/api/v2/dynamic_content/items?page[after]=extra") }; }
            }
            else if (path == "/api/v2/dynamic_content/items/999") { status = 404; response["error"] = "RecordNotFound"; }
            else if (path == "/api/v2/dynamic_content/items/998") { status = 403; response["error"] = "Forbidden"; }
            else if (path == "/api/v2/dynamic_content/items/997") { status = 404; response["error"] = "InvalidEndpoint"; }
            else if (path == "/api/v2/dynamic_content/items/1") response["item"] = Item(1);
            else if (path.StartsWith("/api/v2/dynamic_content/items/1/variants") && c.Request.HttpMethod != "GET")
            {
                using var reader = new StreamReader(c.Request.InputStream); var body = JObject.Parse(await reader.ReadToEndAsync()); var variant = (JObject)body["variant"]!;
                german ??= new JObject { ["id"] = 20, ["locale_id"] = 2, ["default"] = false, ["active"] = true };
                foreach (var p in variant.Properties()) german[p.Name] = p.Value.DeepClone();
                response["variant"] = german.DeepClone(); Writes++;
            }
            else { status = 404; response["error"] = "InvalidEndpoint"; }
            var bytes = Encoding.UTF8.GetBytes(response.ToString()); c.Response.StatusCode = status; c.Response.ContentType = "application/json"; c.Response.ContentLength64 = bytes.Length; await c.Response.OutputStream.WriteAsync(bytes); c.Response.Close();
        }
        public void Dispose() { listener.Stop(); listener.Close(); worker.GetAwaiter().GetResult(); }
    }
}
