using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses
{
    public class ArticleResponseWrapper<T>
    {
        public T Article { get; set; }
    }
}
