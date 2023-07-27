using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses
{
    public class AllLocalesResponse
    {
        [Display("Locales")]
        public List<string> Locales { get; set; }

        [Display("Default locale")]
        [JsonPropertyName("default_locale")]
        public string DefaultLocale { get; set; }
    }
}
