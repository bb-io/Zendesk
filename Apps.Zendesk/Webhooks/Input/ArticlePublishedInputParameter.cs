using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
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

        [Display("Brand ID")]
        public string? BrandId { get; set; }

        [Display("Account ID")]
        public string? AccountId { get; set; }

        [Display("Required Label")]
        [DataSource(typeof(LabelNameDataHandler))]
        public string? RequiredLabel { get; set; }
    }
}
