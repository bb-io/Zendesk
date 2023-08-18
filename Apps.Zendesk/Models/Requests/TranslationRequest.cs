using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Requests
{
    public class TranslationRequest
    {
        [DataSource(typeof(LocaleDataHandler))]
        [Display("Locale")]
        public string Locale { get; set; }

        [Display("Title")]
        [JsonProperty("title")]
        public string? Title { get; set; }

        [Display("Content")]
        [JsonProperty("body")]
        public string? Body { get; set; }

        [Display("Is draft")]
        [JsonProperty("draft")]
        public bool? Draft { get; set; }

        [Display("Is outdated")]
        [JsonProperty("outdated")]
        public bool? Outdated { get; set; }

        public object Convert(bool isLocaleMissing)
        {
            var localeInRequest = isLocaleMissing ? Locale : null;
            return new
            {
                translation = new
                {
                    locale = localeInRequest,
                    title = Title,
                    body = Body,
                    draft = Draft,
                    outdated = Outdated
                }
            };
        }

    }
}
