using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses
{
    public class LocalesResponse
    {
        [Display("Locales")]
        public List<string> Locales { get; set; }
    }
}
