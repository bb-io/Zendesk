using Blackbird.Applications.Sdk.Common;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Zendesk.Models.Responses
{
    public class FileResponse
    {
        [Display("File")]
        public File File { get; set; }
    }
}
