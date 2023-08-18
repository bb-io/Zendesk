using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Identifiers
{
    public class HoursIdentifier
    {
        [Display("Updated in the last hours")]
        public int Hours { get; set; }
    }
}
