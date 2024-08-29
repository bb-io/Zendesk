using Newtonsoft.Json;

namespace Apps.Zendesk.Webhooks.Payload.Articles;

public class PublishEvent
{
    [JsonProperty("author_id")]
    public string AuthorId { get; set; }
    
    [JsonProperty("category_id")]
    public string CategoryId { get; set; }
    
    public string Locale { get; set; }
    
    [JsonProperty("section_id")]
    public string SectionId { get; set; }
    
    public string Title { get; set; }
}