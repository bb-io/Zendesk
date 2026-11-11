using Newtonsoft.Json;

namespace Apps.Zendesk.Models.DynamicContent;

public class SupportLocale
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("locale")]
    public string LocaleCode { get; set; } = string.Empty;

    [JsonProperty("name")]
    public string Name { get; set; } = string.Empty;
}
