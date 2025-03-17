using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses;

public class AuthorChangedResponse : ArticleResponse
{
    [Display("New author ID")]
    public string AuthorId { get; set; }

    [Display("Category")]
    public string CategoryId { get; set; }

    [Display("Locale")]
    public string Locale { get; set; }

    [Display("Section")]
    public string SectionId { get; set; }

    [Display("Title")]
    public string Title { get; set; }

    [Display("Labels")]
    public List<string> Labels { get; set; }
}