using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Polling.Models;

public class OnLabelsAddedInput
{
    [DataSource(typeof(LabelNameDataHandler))]
    public IEnumerable<string> Labels { get; set; }
}