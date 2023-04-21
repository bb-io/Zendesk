﻿using Apps.Zendesk.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.OpenAI.Models.Responses
{
    public class ListArticlesResponse
    {
        public IEnumerable<ArticleDto> Articles { get; set; }
    }
}
