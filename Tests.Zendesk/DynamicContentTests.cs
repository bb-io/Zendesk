using Apps.Zendesk.Actions;
using Apps.Zendesk.Models.Requests;
using Apps.Zendesk.Models.DynamicContent;
using Apps.Zendesk.Utils;
using Blackbird.Filters.Transformations;
using Newtonsoft.Json;
using ZendeskTests.Base;

namespace Tests.Zendesk;

[TestClass]
public class DynamicContentTests : TestBase
{
    private const string TestItemName = "Test Dynamic";

    [TestMethod]
    public void DynamicContentHtmlConverter_preserves_text_and_html_content()
    {
        var item = new DynamicContentItem
        {
            Id = "123",
            Name = "Test",
            Placeholder = "{{dc.test}}"
        };
        var variant = new DynamicContentVariant
        {
            Id = "456",
            Content = "First <literal> line\nSecond & line"
        };

        var textHtml = DynamicContentHtmlConverter.Convert(
            item, variant, "en-US", DynamicContentHtmlConverter.TextFormat,
            "https://example.zendesk.com", "https://example.zendesk.com/admin");
        var parsedText = DynamicContentHtmlConverter.Parse(textHtml);

        Assert.AreEqual(variant.Content, parsedText.Content);
        Assert.AreEqual(item.Id, parsedText.DynamicContentId);
        Assert.AreEqual(variant.Id, parsedText.SourceVariantId);

        variant.Content = "<p>First <strong>HTML</strong> line</p>";
        var richHtml = DynamicContentHtmlConverter.Convert(
            item, variant, "en-US", DynamicContentHtmlConverter.HtmlFormat,
            "https://example.zendesk.com", "https://example.zendesk.com/admin");
        var parsedHtml = DynamicContentHtmlConverter.Parse(richHtml);

        Assert.AreEqual(variant.Content, parsedHtml.Content);
    }

    [TestMethod]
    public async Task SearchDynamicContent_works()
    {
        var actions = new DynamicContentActions(InvocationContext, FileManager);

        var result = await actions.SearchDynamicContent(new SearchDynamicContentRequest
        {
            Query = TestItemName
        });

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        Assert.AreEqual(result.Items.Count, result.TotalCount);
        Assert.IsTrue(result.Items.Any(x =>
            x.Name.Equals(TestItemName, StringComparison.OrdinalIgnoreCase)));
    }

    [TestMethod]
    public async Task FindDynamicContent_by_ID_and_placeholder_works()
    {
        var actions = new DynamicContentActions(InvocationContext, FileManager);
        var item = await GetTestItem(actions);

        var byId = await actions.FindDynamicContent(new FindDynamicContentRequest
        {
            DynamicContentId = item.Id
        });
        var byPlaceholder = await actions.FindDynamicContent(new FindDynamicContentRequest
        {
            Placeholder = item.Placeholder
        });

        Console.WriteLine(JsonConvert.SerializeObject(byId, Formatting.Indented));
        Assert.IsTrue(byId.Found);
        Assert.AreEqual(item.Id, byId.Item?.Id);
        Assert.IsTrue(byPlaceholder.Found);
        Assert.AreEqual(item.Id, byPlaceholder.Item?.Id);
    }

    [TestMethod]
    public async Task DownloadDynamicContent_default_text_variant_works()
    {
        var actions = new DynamicContentActions(InvocationContext, FileManager);
        var item = await GetTestItem(actions);

        var result = await actions.DownloadDynamicContent(new DownloadDynamicContentRequest
        {
            DynamicContentId = item.Id,
            ContentFormat = "Text"
        });

        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        var html = FileManager.ReadOutputAsString(result.Content);
        using var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(html));
        var loadResult = Transformation.Load(stream, result.Content.Name);

        Assert.IsTrue(loadResult.Success, loadResult.Error);
        var sourceResult = loadResult.Value.Source();
        Assert.IsTrue(sourceResult.Success, sourceResult.Error);
        Assert.AreEqual(item.Id, sourceResult.Value.SystemReference.ContentId);
        Assert.AreEqual(result.SourceLanguage, sourceResult.Value.Language);
        Assert.IsTrue(sourceResult.Value.TextUnits.Any(x => x.Key == $"{item.Id}-content"));
    }

    private static async Task<Apps.Zendesk.Models.DynamicContent.DynamicContentItem> GetTestItem(
        DynamicContentActions actions)
    {
        var search = await actions.SearchDynamicContent(new SearchDynamicContentRequest
        {
            Query = TestItemName
        });

        return search.Items.FirstOrDefault(x =>
                   x.Name.Equals(TestItemName, StringComparison.OrdinalIgnoreCase))
               ?? throw new AssertFailedException(
                   $"Dynamic content item '{TestItemName}' was not found in the connected Zendesk instance.");
    }
}
