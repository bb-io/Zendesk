using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using HtmlAgilityPack;
using System.Text;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using System.Web;

namespace Apps.Zendesk.Models.Requests;

public class FileTranslationRequest
{
    [DataSource(typeof(LocaleDataHandler))]
    [Display("Locale")]
    public string Locale { get; set; }

    [Display("HTML file")]
    public FileReference File { get; set; }

    [Display("Is draft")]
    public bool? Draft { get; set; }

    [Display("Is outdated")]
    public bool? Outdated { get; set; }

    public static string ExtractBlackbirdId(byte[] fileBytes)
    {
        var fileString = Encoding.UTF8.GetString(fileBytes);
        var doc = new HtmlDocument();
        doc.LoadHtml(fileString);
        var referenceId = doc.DocumentNode.SelectSingleNode("//meta[@name='blackbird-reference-id']")?.GetAttributeValue("content", null);

        if (referenceId is null)
            throw new Exception("Blackbird reference ID not found in the HTML file.");

        return referenceId;
    }

    public object Convert(byte[] fileBytes, bool isLocaleMissing)
    {
        var fileString = Encoding.UTF8.GetString(fileBytes);
        var localeInRequest = isLocaleMissing ? Locale : null;
        var doc = new HtmlDocument();
        doc.LoadHtml(fileString);

        var title = HttpUtility.HtmlDecode(doc.DocumentNode.SelectSingleNode("html/head/title")?.InnerText);
        var body = doc.DocumentNode.SelectSingleNode("/html/body")?.InnerHtml;

        if (title is null || body is null)
            throw new Exception("Translation HTML file is in a wrong format, please check if it is not corrupted");
            
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