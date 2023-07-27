using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Requests
{
    public class ListArticlesWithLocaleRequest
    {
        public string Locale { get; set; }

        [Display("Changed in the last hours")]
        public int? Hours { get; set; }
        
    }
}
