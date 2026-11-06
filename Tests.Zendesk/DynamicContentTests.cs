using Apps.Zendesk;
using Apps.Zendesk.Actions;
using Apps.Zendesk.DataSourceHandlers;
using Apps.Zendesk.Models;
using Apps.Zendesk.Polling;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Filters.Transformations;
using System.Text;

namespace ZendeskTests;

[TestClass]
public class DynamicContentTests
{
    [TestMethod]
    public void ContentFormatUsesStaticPicker()
    {
        var attribute = typeof(DynamicDownloadInput).GetProperty(nameof(DynamicDownloadInput.Format))!
            .GetCustomAttributes(typeof(StaticDataSourceAttribute), false).Cast<StaticDataSourceAttribute>().Single();
        Assert.AreEqual(typeof(DynamicFormatDataHandler), attribute.DataSourceHandler);
        CollectionAssert.AreEqual(new[] { "Text", "HTML" }, new DynamicFormatDataHandler().GetData().Select(x => x.Value).ToArray());
    }
    private static DynamicItem Item(int id, string text = "source") => new() { Id = id.ToString(), Name = "Example", Placeholder = "{{dc.example}}", DefaultLocaleId = 1, DefaultLanguage = "en-US", Variants = new() { new() { Id = $"{id}0", LocaleId = 1, Language = "en-US", Default = true, Content = text }, new() { Id = $"{id}1", LocaleId = 2, Language = "de", Content = "Ziel" } } };

    [TestMethod]
    public void PollBaselinesAndOnlyTriggersForSourceChanges()
    {
        var item = Item(1);
        var baseline = DynamicContentPolling.Compare(new[] { item }, null, new());
        Assert.IsFalse(baseline.FlyBird); Assert.IsNull(baseline.Result);
        item.Name = "Renamed"; item.Variants[1].Content = "Changed target";
        var unchanged = DynamicContentPolling.Compare(new[] { item }, baseline.Memory, new());
        Assert.IsFalse(unchanged.FlyBird); Assert.IsNull(unchanged.Result);
        item.Variants[0].Content = "Changed source";
        var changed = DynamicContentPolling.Compare(new[] { item }, unchanged.Memory, new());
        Assert.IsTrue(changed.FlyBird); Assert.AreEqual("updated", changed.Result![0].EventType);
        Assert.IsFalse(DynamicContentPolling.Compare(new[] { item }, changed.Memory, new()).FlyBird);
        Assert.IsFalse(DynamicContentPolling.Compare(new[] { item }, changed.Memory, new() { Language = "fr" }).FlyBird);
        item.Variants[0].Id = "new-variant";
        var variantChanged = DynamicContentPolling.Compare(new[] { item }, changed.Memory, new());
        Assert.AreEqual("updated", variantChanged.Result![0].EventType);
        var languageChanged = DynamicContentPolling.Compare(new[] { item }, variantChanged.Memory, new() { Language = "de" });
        Assert.AreEqual("updated", languageChanged.Result![0].EventType);
        Assert.AreEqual("de", languageChanged.Result[0].SourceLanguage);
    }
    [TestMethod]
    public void PollKeepsDeferredChangesUnacknowledged()
    {
        var baseline = DynamicContentPolling.Compare(Array.Empty<DynamicItem>(), null, new());
        var items = Enumerable.Range(1, 101).Select(x => Item(x)).ToArray();
        var first = DynamicContentPolling.Compare(items, baseline.Memory, new());
        Assert.AreEqual(100, first.Result!.Count); Assert.AreEqual(100, first.Memory.Items.Count);
        var second = DynamicContentPolling.Compare(items, first.Memory, new());
        Assert.AreEqual(1, second.Result!.Count); Assert.AreEqual("created", second.Result[0].EventType);
        Assert.IsFalse(DynamicContentPolling.Compare(items, second.Memory, new()).FlyBird);
    }
    [TestMethod]
    public void PollUsesEmptySourceTextForNullContent()
    {
        var item = Item(1);
        var baseline = DynamicContentPolling.Compare(new[] { item }, null, new());
        item.Variants[0].Content = null!;
        var changed = DynamicContentPolling.Compare(new[] { item }, baseline.Memory, new());
        Assert.AreEqual("", changed.Result![0].SourceText);
        Assert.IsFalse(DynamicContentPolling.Compare(new[] { item }, changed.Memory, new()).FlyBird);
    }
    [DataTestMethod]
    [DataRow("hc_toggle_theme")]
    [DataRow("dc.hc_toggle_theme")]
    [DataRow("{{dc.hc_toggle_theme}}")]
    public void PlaceholderFormsMatch(string input) => Assert.AreEqual("hc_toggle_theme", DynamicContentApi.NormalizePlaceholder(input));

    [DataTestMethod]
    [DataRow("Text", "A <tag> & text\nsecond line")]
    [DataRow("HTML", "<p>A &amp; <strong>B</strong></p>")]
    public void FileRetainsContentAndRoundtripMetadata(string format, string value)
    {
        var item = Item(123, value); var variant = item.Variants[0];
        var html = DynamicContentFile.Create(item, variant, format, "https://example.zendesk.com");
        StringAssert.Contains(html, "example.zendesk.com:dynamic-content:123");
        StringAssert.Contains(html, "data-blackbird-key=\"dynamic-content-123-content\"");
        var extracted = DynamicContentFile.Extract(html);
        Assert.AreEqual("123", extracted.Id); Assert.AreEqual(value, extracted.Content);
        using var stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
        var loaded = Transformation.Load(stream, "example.html", "text/html");
        Assert.IsNotNull(loaded.Value, loaded.Error);
        var target = loaded.Target(); Assert.IsTrue(target.Success, target.Error);
        using var output = target.Value.ToStream(); using var reader = new StreamReader(output);
        var roundtrip = DynamicContentFile.Extract(reader.ReadToEnd());
        Assert.AreEqual(value, roundtrip.Content);
    }
}
