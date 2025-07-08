using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using HtmlAgilityPack;
using System.Text;
using Blackbird.Applications.Sdk.Common.Files;
using System.Web;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Zendesk.Models.Requests;

public class FileTranslationRequest : IUploadContentInput
{
    [DataSource(typeof(LocaleDataHandler))]
    [Display("Locale")]
    public string Locale { get; set; }

    [Display("Content")]
    public FileReference Content { get; set; }

    [Display("Is draft")]
    public bool? Draft { get; set; }

    [Display("Is outdated")]
    public bool? Outdated { get; set; }

    [DataSource(typeof(ArticleDataHandler))]
    [Display("Article ID")]
    public string? ContentId { get; set; }

    public static string? ExtractBlackbirdId(string html)
    {
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        var referenceId = doc.DocumentNode.SelectSingleNode($"//meta[@name='{Constants.Constants.BlackbirdReferenceId}']")?.GetAttributeValue("content", null) ?? null;

        return referenceId;
    }

    public object Convert(string html, bool isLocaleMissing)
    {
        var localeInRequest = isLocaleMissing ? Locale : null;
        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var title = HttpUtility.HtmlDecode(doc.DocumentNode.SelectSingleNode("html/head/title")?.InnerText);
        var body = doc.DocumentNode.SelectSingleNode("/html/body")?.InnerHtml;

        if (title is null || body is null)
            throw new PluginMisconfigurationException("Translation HTML file is in a wrong format, please check if it is not corrupted");
            
        return new
        {
            translation = new
            {
                locale = localeInRequest,
                title = title,
                body = body,
                draft = Draft,
                outdated = Outdated
            }
        };
    }
}