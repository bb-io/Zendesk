using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using HtmlAgilityPack;
using System.Text;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Zendesk.Models.Requests;

public class FileTranslationRequest
{
    [DataSource(typeof(LocaleDataHandler))]
    [Display("Locale")]
    public string Locale { get; set; }

    [Display("HTML file")]
    public File File { get; set; }

    [Display("Is draft")]
    public bool? Draft { get; set; }

    [Display("Is outdated")]
    public bool? Outdated { get; set; }

    public object Convert(bool isLocaleMissing)
    {
        var localeInRequest = isLocaleMissing ? Locale : null;
        var fileString = Encoding.UTF8.GetString(File.Bytes);
        var doc = new HtmlDocument();
        doc.LoadHtml(fileString);
        var title = doc.DocumentNode.SelectSingleNode("html/head/title")?.InnerText;
        var body = doc.DocumentNode.SelectSingleNode("/html/body")?.InnerHtml;

        if (title is null || body is null)
            throw new("Translation HTML file is in a wrong format, please check if it is not corrupted");
            
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