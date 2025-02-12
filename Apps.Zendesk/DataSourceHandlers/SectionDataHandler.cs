using Apps.Zendesk.Models.Responses.Wrappers;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.Zendesk.DataSourceHandlers;

public class SectionDataHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public SectionDataHandler(InvocationContext invocationContext) : base(invocationContext) { }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(InvocationContext);
        var request = new ZendeskRequest("/api/v2/help_center/sections", Method.Get);
        var sections = (await client.GetPaginated<MultipleSections>(request)).SelectMany(x => x.Sections);

        return sections
            .OrderByDescending(x => x.UpdatedAt)
            .ToDictionary(x => x.Id.ToString(), x => x.Name);
    }
}