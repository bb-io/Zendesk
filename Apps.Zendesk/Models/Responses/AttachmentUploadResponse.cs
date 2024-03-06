using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Responses
{
    public class AttachmentUploadResponse
    {
        [JsonProperty("article_attachment")]
        public Attachment Attachment { get; set; }
    }

}
