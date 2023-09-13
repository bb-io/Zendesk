using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.DataSourceHandlers
{
    public class PriorityDataHandler : IDataSourceHandler
    {
        public Dictionary<string, string> GetData(DataSourceContext context)
        {
            return new Dictionary<string, string>
            {
                { "low", "Low" },
                { "normal", "Normal" },
                { "high", "High" },
                { "urgent", "Urgent" }
            };
        }
    }
}
