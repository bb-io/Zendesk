using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Requests
{
    public class GetArticleTranslationsRequest
    {
        [Display("Article ID")]
        public string ArticleId { get; set; }
    }
}
