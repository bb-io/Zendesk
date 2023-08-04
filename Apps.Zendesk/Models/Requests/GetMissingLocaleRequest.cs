using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Requests
{
    public class GetMissingLocaleRequest
    {
        [Display("Article ID")]
        public string ArticleId { get; set; }
    }
}
