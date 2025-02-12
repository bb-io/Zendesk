using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses;
public class PaginatedResultResponse<T>
{
    [JsonProperty("next_page")]
    public string? Next { get; set; }

    public IEnumerable<T> Results { get; set; }
}
