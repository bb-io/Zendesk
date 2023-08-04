using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Requests
{
    public class GetArticleRequest
    {
        [Display("Article ID")]
        public string ArticleId { get; set; }

        public string? Locale { get; set; }
    }
}
