using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses.Wrappers
{
    public class MultipleUsers : PaginatedResponse
    {
        public IEnumerable<NamedResource> Users { get; set; }
    }
}
