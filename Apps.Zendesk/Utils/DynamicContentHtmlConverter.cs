using Apps.Zendesk.Models.DynamicContent;
using HtmlAgilityPack;
using System.Net;

namespace Apps.Zendesk.Utils;

public static class DynamicContentHtmlConverter
{
    public const string TextFormat = "Text";
    public const string HtmlFormat = "HTML";

    public sealed record ParsedContent(
        string? DynamicContentId,
        string? SourceVariantId,
        string? Placeholder,
        string ContentFormat,
        string Content);

    public static string Convert(
        DynamicContentItem item,
        DynamicContentVariant variant,
        string sourceLanguage,
        string contentFormat,
        string systemReference,
        string adminUrl)
    {
        var document = new HtmlDocument();
        var html = document.CreateElement("html");
        html.SetAttributeValue("lang", sourceLanguage);
        document.DocumentNode.AppendChild(html);

        var head = document.CreateElement("head");
        html.AppendChild(head);
        AddMeta(document, head, "blackbird-ucid", item.Id);
        AddMeta(document, head, "blackbird-content-name", item.Name);
        AddMeta(document, head, "blackbird-admin-url", adminUrl);
        AddMeta(document, head, "blackbird-system-name", "Zendesk");
        AddMeta(document, head, "blackbird-system-ref", systemReference);
        AddMeta(document, head, "zendesk-dynamic-content-source-variant-id", variant.Id);
        AddMeta(document, head, "zendesk-dynamic-content-placeholder", item.Placeholder);
        AddMeta(document, head, "zendesk-dynamic-content-format", contentFormat);

        var body = document.CreateElement("body");
        body.SetAttributeValue("its-rev-tool", "Zendesk");
        body.SetAttributeValue("its-rev-tool-ref", systemReference);
        html.AppendChild(body);

        var container = document.CreateElement("div");
        container.SetAttributeValue("data-zendesk-dynamic-content-id", item.Id);
        container.SetAttributeValue("data-zendesk-variant-id", variant.Id);
        container.SetAttributeValue("data-zendesk-placeholder", item.Placeholder);
        container.SetAttributeValue("data-zendesk-content-format", contentFormat);
        body.AppendChild(container);

        var content = document.CreateElement(contentFormat == TextFormat ? "pre" : "div");
        content.SetAttributeValue("data-zendesk-field-id", "content");
        content.SetAttributeValue("data-zendesk-field-type", contentFormat);
        content.SetAttributeValue("data-blackbird-key", $"{item.Id}-content");
        content.InnerHtml = contentFormat == TextFormat
            ? WebUtility.HtmlEncode(variant.Content)
            : variant.Content;
        container.AppendChild(content);

        return document.DocumentNode.OuterHtml;
    }

    public static ParsedContent Parse(string html)
    {
        var document = new HtmlDocument();
        document.LoadHtml(html);

        var contentNode = document.DocumentNode.SelectSingleNode(
            "//*[@data-zendesk-field-id='content']");
        if (contentNode is null)
        {
            throw new InvalidDataException(
                "The file does not contain the Zendesk dynamic content field. Check that the file was produced by Download dynamic content.");
        }

        var format = contentNode.GetAttributeValue("data-zendesk-field-type", null)
            ?? GetMeta(document, "zendesk-dynamic-content-format");
        if (format?.Equals(TextFormat, StringComparison.OrdinalIgnoreCase) == true)
            format = TextFormat;
        else if (format?.Equals(HtmlFormat, StringComparison.OrdinalIgnoreCase) == true)
            format = HtmlFormat;
        else
            throw new InvalidDataException("The dynamic content format metadata must be Text or HTML.");

        var container = contentNode.Ancestors("div").FirstOrDefault(x =>
            x.Attributes["data-zendesk-dynamic-content-id"] is not null);
        var itemId = container?.GetAttributeValue("data-zendesk-dynamic-content-id", null)
            ?? GetMeta(document, "blackbird-ucid");
        var variantId = container?.GetAttributeValue("data-zendesk-variant-id", null)
            ?? GetMeta(document, "zendesk-dynamic-content-source-variant-id");
        var placeholder = container?.GetAttributeValue("data-zendesk-placeholder", null)
            ?? GetMeta(document, "zendesk-dynamic-content-placeholder");

        var content = format == TextFormat
            ? HtmlEntity.DeEntitize(contentNode.InnerText)
            : contentNode.InnerHtml;

        return new ParsedContent(itemId, variantId, placeholder, format, content);
    }

    private static void AddMeta(HtmlDocument document, HtmlNode head, string name, string value)
    {
        var meta = document.CreateElement("meta");
        meta.SetAttributeValue("name", name);
        meta.SetAttributeValue("content", value);
        head.AppendChild(meta);
    }

    private static string? GetMeta(HtmlDocument document, string name) =>
        document.DocumentNode.SelectSingleNode($"//meta[@name='{name}']")
            ?.GetAttributeValue("content", null);
}
