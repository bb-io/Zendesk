using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses;

public class ArticlePublishedResponse : ArticleResponse
{
    [Display("Author")]
    public string AuthorId { get; set; }

    public string Locale { get; set; }

    [Display("Section")]
    public string SectionId { get; set; }
    public string Title { get; set; }

    [Display("Brand ID")]
    public string BrandId { get; set; }

    [Display("Account ID")]
    public string AccountId { get; set; }

    [Display("Labels")]
    public IEnumerable<string> Labels { get; set; }

    [Display("Missing locales")]
    public List<string> MissingLocales { get; set; }

    [Display("Outdated locales")]
    public List<string> OutdatedLocales { get; set; }

    [Display("Source locale")]
    public string SourceLocale { get; set; }
}