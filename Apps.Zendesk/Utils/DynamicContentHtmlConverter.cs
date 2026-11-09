using Apps.Zendesk.Models.DynamicContent;
using HtmlAgilityPack;
using System.Net;

namespace Apps.Zendesk.Utils;

public static class DynamicContentHtmlConverter
{
    public const string TextFormat = "Text";
    public const string HtmlFormat = "HTML";

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

    private static void AddMeta(HtmlDocument document, HtmlNode head, string name, string value)
    {
        var meta = document.CreateElement("meta");
        meta.SetAttributeValue("name", name);
        meta.SetAttributeValue("content", value);
        head.AppendChild(meta);
    }
}
