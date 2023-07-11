using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Webhooks.Responses
{
    public class VoteRemovedResponse : ArticleResponse
    {
        [Display("Vote ID")]
        public string Id { get; set; }
    }
}
