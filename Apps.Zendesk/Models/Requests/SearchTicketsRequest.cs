using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Requests
{
    public class SearchTicketsRequest
    {
        [Display("Status")]
        [DataSource(typeof(StatusDataHandler))]
        public string? Status { get; set; }

        [Display("Priority")]
        [DataSource(typeof(PriorityDataHandler))]
        public string? Priority { get; set; }
    }
}
