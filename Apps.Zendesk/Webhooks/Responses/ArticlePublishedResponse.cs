using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Webhooks.Responses
{
    public class ArticlePublishedResponse : ArticleResponse
    {
        [Display("Author ID")]
        public string AuthorId { get; set; }

        [Display("Category ID")]
        public string CategoryId { get; set; }

        public string Locale { get; set; }

        [Display("Section ID")]
        public string SectionId { get; set; }
        public string Title { get; set; }
    }
}
