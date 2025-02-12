using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Zendesk.Models.Responses;

public class FileResponse
{
    [Display("Content file")]
    public FileReference File { get; set; }
}