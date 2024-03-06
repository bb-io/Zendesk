using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses
{
    public class Attachment
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("article_id")]
        public string ArticleId { get; set; }

        [JsonProperty("display_file_name")]
        public string DisplayFileName { get; set; }

        [JsonProperty("file_name")]
        public string FileName { get; set; }

        [JsonProperty("content_url")]
        public string ContentUrl { get; set; }

        [JsonProperty("relative_path")]
        public string RelativePath { get; set; }

        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("inline")]
        public bool Inline { get; set; }

    }
}
