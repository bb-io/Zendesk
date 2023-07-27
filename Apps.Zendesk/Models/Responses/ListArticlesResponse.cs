using Apps.Zendesk.Dtos;
using Apps.Zendesk.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apps.OpenAI.Models.Responses
{
    public class ListArticlesResponse
    {
        public IEnumerable<Article> Articles { get; set; }
    }
}
