using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses
{
    public class SearchResponse<T>
    {
        public IEnumerable<T> Results { get; set; }
    }
}
