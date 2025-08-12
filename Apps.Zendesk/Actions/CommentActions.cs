using Apps.Zendesk.Models.Identifiers;
using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Webhooks.Payload.Articles;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.Actions
{
    [ActionList("Comments")]
    public class CommentActions : BaseInvocable
    {
        private ZendeskClient Client { get; }

        public CommentActions(InvocationContext invocationContext) : base(invocationContext)
        {
            Client = new ZendeskClient(invocationContext);
        }

       
    }
}
