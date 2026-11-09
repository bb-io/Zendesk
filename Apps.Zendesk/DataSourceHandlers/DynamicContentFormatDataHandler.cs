using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.DataSourceHandlers;

public class DynamicContentFormatDataHandler : IDataSourceItemHandler
{
    public IEnumerable<DataSourceItem> GetData(DataSourceContext context) =>
    [
        new DataSourceItem("Text", "Text"),
        new DataSourceItem("HTML", "HTML")
    ];
}
