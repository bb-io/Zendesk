using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Requests
{
    public class ListArticlesRequest
    {
        [Display("Changed in the last hours")]
        public int? Hours { get; set; }
    }
}
