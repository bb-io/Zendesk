using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Requests
{
    public class ListArticlesWithLocaleRequest
    {
        [DataSource(typeof(LocaleDataHandler))]
        public string Locale { get; set; }

        [Display("Changed in the last hours")]
        public int? Hours { get; set; }
        
    }
}
