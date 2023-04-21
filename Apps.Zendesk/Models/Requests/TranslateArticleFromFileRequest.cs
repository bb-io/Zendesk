using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Requests
{
    public class TranslateArticleFromFileRequest
    {
        public string Locale { get; set; }

        public string ArticleId { get; set; }

        public byte[] File { get; set; }
    }
}
