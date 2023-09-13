using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Identifiers
{
    public class OptionalCategoryIdentifier
    {
        [Display("Category")]
        [DataSource(typeof(CategoryDataHandler))]
        public string? Id { get; set; }
    }
}
