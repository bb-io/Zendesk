using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.DataSourceHandlers
{
    public class AdminDataHandler : BaseInvocable, IDataSourceHandler
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        public AdminDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

        public Dictionary<string, string> GetData(DataSourceContext context)
        {
            var client = new ZendeskClient(InvocationContext);
            var request = new ZendeskRequest("/api/v2/users", Method.Get, Creds);
            request.AddQueryParameter("role", "admin");
            var users = client.GetPaginated<MultipleUsers>(request).SelectMany(x => x.Users);

            return users
                .OrderBy(x => x.Name)
                .ToDictionary(x => x.Id, x => x.Name);
        }
    }
}
