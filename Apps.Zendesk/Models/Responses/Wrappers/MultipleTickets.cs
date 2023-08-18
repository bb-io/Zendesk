using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses.Wrappers
{
    public class MultipleTickets : PaginatedResponse
    {
        public IEnumerable<Ticket> Tickets { get; set; }
    }
}
