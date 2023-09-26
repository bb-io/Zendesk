using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses;

public class ArticlePublishedResponse : ArticleResponse
{
    [Display("Author")]
    public string AuthorId { get; set; }

    [Display("Category")]
    public string CategoryId { get; set; }

    public string Locale { get; set; }

    [Display("Section")]
    public string SectionId { get; set; }
    public string Title { get; set; }
}