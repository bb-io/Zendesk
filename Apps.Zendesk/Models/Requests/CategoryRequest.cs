using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Requests
{
    public class CategoryRequest
    {
        [Display("Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Display("Description")]
        [JsonProperty("description")]
        public string? Description { get; set; }

        [Display("Position")]
        [JsonProperty("position")]
        public int? Position { get; set; }
    }
}
