﻿using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Responses
{
    public class ArticlePublishedResponse : ArticleResponse
    {
        [Display("Author ID")]
        public string AuthorId { get; set; }

        [Display("Category ID")]
        public string CategoryId { get; set; }

        public string Locale { get; set; }

        [Display("Section ID")]
        public string SectionId { get; set; }
        public string Title { get; set; }
    }
}
