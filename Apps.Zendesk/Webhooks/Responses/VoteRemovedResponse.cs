using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses
{
    public class VoteRemovedResponse : ArticleResponse
    {
        [Display("Vote ID")]
        public string Id { get; set; }
    }
}
