using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses;

public class VoteResponse : ArticleResponse
{
    [Display("Vote ID")]
    public string Id { get; set; }

    [Display("User ID")]
    public string UserId { get; set; }

    public int Value { get; set; }
}