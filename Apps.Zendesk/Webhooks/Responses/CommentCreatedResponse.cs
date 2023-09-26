using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses;

public class CommentCreatedResponse : ArticleResponse
{
    [Display("Author ID")]
    public string AuthorId { get; set; }

    [Display("Comment ID")]
    public string Id { get; set; }
    public string Locale { get; set; }
}