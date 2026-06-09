using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Polling.Models;

public class OnLabelsAddedInput
{
    [DataSource(typeof(LabelNameDataHandler))]
    public IEnumerable<string> Labels { get; set; }

    [Display("Deep search",
        Description = "When enabled, queries articles directly by label name in addition to the incremental feed. " +
                      "Use this if label changes are not triggering the event on your Zendesk instance.")]
    public bool? DeepSearch { get; set; }
}