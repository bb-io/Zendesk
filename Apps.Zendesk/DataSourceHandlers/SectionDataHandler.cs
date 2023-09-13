using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers
{
    public class SectionDataHandler : BaseInvocable, IDataSourceHandler
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        public SectionDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

        public Dictionary<string, string> GetData(DataSourceContext context)
        {
            var client = new ZendeskClient(InvocationContext);
            var request = new ZendeskRequest("/api/v2/help_center/sections", Method.Get, Creds);
            var sections = client.GetPaginated<MultipleSections>(request).SelectMany(x => x.Sections);

            return sections
                .OrderByDescending(x => x.UpdatedAt)
                .ToDictionary(x => x.Id.ToString(), x => x.Name);
        }
    }
}
