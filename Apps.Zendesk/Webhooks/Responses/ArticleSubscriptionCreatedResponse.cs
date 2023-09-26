using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses;

public class ArticleSubscriptionCreatedResponse : ArticleResponse
{
    [Display("Subscription ID")]
    public string Id { get; set; }

    [Display("User ID")]
    public string UserId { get; set; }
}