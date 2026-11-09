using Apps.Zendesk.Models.Responses;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.DynamicContent;

public class DynamicContentItemsResponse : PaginatedResponse
{
    [JsonProperty("items")]
    public List<DynamicContentItem> Items { get; set; } = [];
}

public class DynamicContentItemResponse
{
    [JsonProperty("item")]
    public DynamicContentItem Item { get; set; } = new();
}

public class SupportLocalesResponse
{
    [JsonProperty("locales")]
    public List<SupportLocale> Locales { get; set; } = [];
}
