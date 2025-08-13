using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Requests
{
    public class TicketCommentFiltersInput
    {
        [Display("Ignore if body contains")]
        public string? IgnoreIfBodyContains { get; set; }

        [Display("Only public comments")]
        public bool? OnlyPublic { get; set; }

        [Display("Ignore if web service comment")]         // if true — ignonre, where detail.via.channel == "web_service"
        public bool? IgnoreIfViaWebService { get; set; }
    }
}
