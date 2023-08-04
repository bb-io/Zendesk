using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Requests
{
    public class ListArticlesWithLocaleRequest
    {
        public string Locale { get; set; }

        [Display("Changed in the last hours")]
        public int? Hours { get; set; }
        
    }
}
