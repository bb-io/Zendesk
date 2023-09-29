using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Input
{
    public class ArticlePublishedInputParameter
    {
        [Display("Only source articles")]
        public bool? OnlyIfSource { get; set; }
    }
}
