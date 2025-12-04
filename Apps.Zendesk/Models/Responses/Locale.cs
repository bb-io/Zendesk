using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses;

public class Locale
{
    [JsonProperty("locale")]
    public string LocaleCode { get; set; }
    public string Name { get; set; }

    [JsonProperty("native_name")]
    public string NativeName { get; set; }

    [JsonProperty("presentation_name")]
    public string PresentationName { get; set; }
}
