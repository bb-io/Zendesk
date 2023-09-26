using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses.Wrappers;

public class MultiplePermissionGroups : PaginatedResponse
{
    [JsonProperty("permission_groups")]
    public IEnumerable<NamedResource> PermissionGroups { get; set; }
}