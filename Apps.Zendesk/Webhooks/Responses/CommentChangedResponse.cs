using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses
{
    public class CommentChangedResponse : ArticleResponse
    {

        [Display("Comment ID")]
        public string Id { get; set; }
    }
}
