using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Identifiers
{
    public class SectionIdentifier
    {
        [Display("Section")]
        [DataSource(typeof(SectionDataHandler))]
        public string Id { get; set; }
    }
}
