using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.DataSourceHandlers
{
    public class StatusDataHandler : IDataSourceHandler
    {
        public Dictionary<string, string> GetData(DataSourceContext context)
        {
            return new Dictionary<string, string>
            {
                { "new", "New" },
                { "open", "Open" },
                { "pending", "Pending" },
                { "hold", "Hold" },
                { "solved", "Solved" },
                { "closed", "Closed" }
            };
        }
    }
}
