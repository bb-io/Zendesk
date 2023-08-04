using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Responses
{
    public class BaseResponse
    {
        [Display("Status code")]
        public int StatusCode { get; set; }

        public string Details { get; set; }
    }
}
