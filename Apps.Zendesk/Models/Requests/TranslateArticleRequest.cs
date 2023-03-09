using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Requests
{
    public class TranslateArticleRequest
    {
        public string Locale { get; set; }

        public string ArticleId { get; set; }

        public string TranslatedTitle { get; set; }

        public string TranslatedBody { get; set; }
    }
}
