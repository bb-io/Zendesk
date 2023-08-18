using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.DataSourceHandlers
{
    public class PriorityDataHandler : IDataSourceHandler
    {
        public Dictionary<string, string> GetData(DataSourceContext context)
        {
            return new Dictionary<string, string>
            {
                { "low", "Low" },
                { "normal", "Normal" },
                { "high", "High" },
                { "urgent", "Urgent" }
            };
        }
    }
}
