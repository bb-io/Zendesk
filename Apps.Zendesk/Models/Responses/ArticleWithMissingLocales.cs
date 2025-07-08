using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models.Responses;

public class ArticleWithMissingLocales : Article
{  
    [Display("Missing locales")]
    public List<string> MissingLocales { get; set; }    
}