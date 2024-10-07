using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers
{
    public class LabelDataHandler : BaseInvocable, IDataSourceHandler
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        public LabelDataHandler(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        public Dictionary<string, string> GetData(DataSourceContext context)
        {
            try
            {
                var client = new ZendeskClient(InvocationContext);
                var request = new ZendeskRequest($"/api/v2/help_center/articles/labels", Method.Get, Creds);
                var response = client.Execute<LabelResponse>(request);

                return response.Labels
                    .Where(x => context.SearchString == null ||
                                x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                    .ToDictionary(x => x.Id.ToString(), x => x.Name);
            } catch (Exception ex)
            {
                throw new Exception("Article labels are only available on the Professional and Enterprise plans.");
            }

        }
    }
}
