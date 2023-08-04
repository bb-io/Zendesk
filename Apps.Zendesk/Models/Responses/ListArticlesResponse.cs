using Apps.Zendesk.Models.Responses;

namespace Apps.OpenAI.Models.Responses
{
    public class ListArticlesResponse
    {
        public IEnumerable<Article> Articles { get; set; }
    }
}
