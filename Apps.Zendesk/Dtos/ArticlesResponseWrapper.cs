﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Dtos
{
    public class ArticlesResponseWrapper
    {
        public IEnumerable<ArticleDto> Articles { get; set; }
    }
}