using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses
{
    public class BaseResponse
    {
        [Display("Status code")]
        public int StatusCode { get; set; }

        public string Details { get; set; }
    }
}
