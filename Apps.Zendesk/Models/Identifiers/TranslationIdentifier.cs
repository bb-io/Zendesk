using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Identifiers
{
    public class TranslationIdentifier
    {
        [Display("Translation")]
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
