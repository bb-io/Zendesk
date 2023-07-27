using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apps.Zendesk.Dtos
{
    public class PaginatedResponse
    {
        [JsonPropertyName("next_page")]
        public string? NextPage { get; set; }
    }
}
