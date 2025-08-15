using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Requests
{
    public class TicketCommentsFilterInput
    {
        [Display("Created after")]
        public DateTime? CreatedAfterUtc { get; set; }

        [Display("Created before")]
        public DateTime? CreatedBeforeUtc { get; set; }

        [Display("Newest first")]
        public bool? NewestFirst { get; set; } = true;
    }
}
