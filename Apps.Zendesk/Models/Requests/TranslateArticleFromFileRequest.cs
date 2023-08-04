using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Requests
{
    public class TranslateArticleFromFileRequest
    {
        public string Locale { get; set; }

        [Display("Article ID")]
        public string ArticleId { get; set; }

        public byte[] File { get; set; }

        public bool? Draft { get; set; }
    }
}
