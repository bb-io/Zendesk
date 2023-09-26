using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses;

public class AuthorChangedResponse : ArticleResponse
{
    [Display("New author ID")]
    public string AuthorId { get; set; }
        
}