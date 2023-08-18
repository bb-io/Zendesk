using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses.Wrappers
{
    public class MultipleTranslations : PaginatedResponse
    {
        public IEnumerable<Translation> Translations { get; set; }
    }
}
