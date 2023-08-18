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
using Apps.Zendesk.Models.Responses.Wrappers;

namespace Apps.Zendesk.DataSourceHandlers
{
    public class CategoryDataHandler : BaseInvocable, IDataSourceHandler
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        public CategoryDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

        public Dictionary<string, string> GetData(DataSourceContext context)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest("/api/v2/help_center/categories", Method.Get, Creds);
            var categories = client.GetPaginated<MultipleCategories>(request).SelectMany(x => x.Categories);

            return categories
                .OrderByDescending(x => x.UpdatedAt)
                .ToDictionary(x => x.Id.ToString(), x => x.Name);
        }
    }
}
