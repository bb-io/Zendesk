using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apps.Zendesk.Dtos
{
    public class ArticleResponseWrapper<T>
    {
        public T Article { get; set; }
    }
}
