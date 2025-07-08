using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Zendesk.Models.Responses;

public class FileResponse : IDownloadContentOutput
{
    [Display("Content")]
    public FileReference Content { get; set; }
}