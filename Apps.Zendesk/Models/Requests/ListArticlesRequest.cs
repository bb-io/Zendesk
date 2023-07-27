﻿using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Requests
{
    public class ListArticlesRequest
    {
        [Display("Changed in the last hours")]
        public int? Hours { get; set; }
    }
}
