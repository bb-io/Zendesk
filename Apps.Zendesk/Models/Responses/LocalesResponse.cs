using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Responses
{
    public class LocalesResponse
    {
        [Display("Locales")]
        public List<string> Locales { get; set; }
    }
}
