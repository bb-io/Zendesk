using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses.Wrappers
{
    public class MultiplePermissionGroups : PaginatedResponse
    {
        [JsonProperty("permission_groups")]
        public IEnumerable<NamedResource> PermissionGroups { get; set; }
    }
}
