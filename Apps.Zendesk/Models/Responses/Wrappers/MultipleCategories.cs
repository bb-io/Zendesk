using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses.Wrappers
{
    public class MultipleCategories : PaginatedResponse
    {
        public IEnumerable<Category> Categories { get; set; }
    }
}
