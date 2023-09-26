using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses;

public class CommentPublishResponse : ArticleResponse
{
    [Display("Comment ID")]
    public string Id { get; set; }
    public string Locale { get; set; }
}