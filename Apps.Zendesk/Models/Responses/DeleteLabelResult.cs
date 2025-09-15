using Blackbird.Applications.Sdk.Common;

namespace Apps.Zendesk.Models.Responses
{
    public class DeleteLabelResult
    {
        [Display("Is deleted")]
        public bool IsDeleted { get; set; }
    }
}
