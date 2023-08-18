using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Identifiers
{
    public class LocaleIdentifier
    {
        [DataSource(typeof(LocaleDataHandler))]
        [Display("Locale")]
        public string Locale { get; set; }
    }
}
