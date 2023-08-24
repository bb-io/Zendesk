using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using File = Blackbird.Applications.Sdk.Common.Files.File;

namespace Apps.Zendesk.Models.Responses
{
    public class FileResponse
    {
        [Display("File")]
        public File File { get; set; }
    }
}
