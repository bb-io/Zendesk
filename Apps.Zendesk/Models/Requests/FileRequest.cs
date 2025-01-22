﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Zendesk.Models.Requests
{
    public class FileRequest
    {
        [Display("File")]
        public FileReference File {  get; set; }
    }
}
