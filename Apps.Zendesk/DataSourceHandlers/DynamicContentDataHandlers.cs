using Apps.Zendesk.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Zendesk.DataSourceHandlers;

public class DynamicContentDataHandler(InvocationContext context) : BaseInvocable(context), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken) =>
        (await new DynamicContentApi(new ZendeskClient(InvocationContext)).List()).Where(x => string.IsNullOrWhiteSpace(context.SearchString) || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase) || x.Placeholder.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase)).Select(x => new DataSourceItem(x.Id, x.Name));
}
public class SupportLanguageDataHandler(InvocationContext context) : BaseInvocable(context), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken) =>
        (await new DynamicContentApi(new ZendeskClient(InvocationContext)).Locales()).Where(x => string.IsNullOrWhiteSpace(context.SearchString) || x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase) || x.Code.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase)).Select(x => new DataSourceItem(x.Code, x.Name));
}
public class DynamicFormatDataHandler : IDataSourceHandler
{
    public Dictionary<string, string> GetData(DataSourceContext context) => new() { ["Text"] = "Text", ["HTML"] = "HTML" };
}
