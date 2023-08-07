using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Requests.Chat;

public class SessionData
{
    [JsonProperty("browser")] public string? Browser { get; set; }

    [JsonProperty("city")] public string? City { get; set; }

    [JsonProperty("country_code")]
    [Display("Country code")]
    public string? CountryCode { get; set; }

    [JsonProperty("country_name")]
    [Display("Country name")]
    public string? CountryName { get; set; }

    [JsonProperty("start_date")]
    [Display("Start date")]
    public DateTime? StartDate { get; set; }

    [JsonProperty("end_date")]
    [Display("End date")]
    public DateTime? EndDate { get; set; }

    [JsonProperty("id")] [Display("ID")] public string? Id { get; set; }

    [JsonProperty("ip")] [Display("IP")] public string? Ip { get; set; }

    [JsonProperty("platform")] public string? Platform { get; set; }

    [JsonProperty("region")] public string? Region { get; set; }

    [JsonProperty("user_agent")]
    [Display("User agent")]
    public string? UserAgent { get; set; }
}