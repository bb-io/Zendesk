using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses.Wrappers
{
    public class MultipleUserSegments : PaginatedResponse
    {
        [JsonProperty("user_segments")]
        public IEnumerable<NamedResource> UserSegments { get; set; }
    }
}
