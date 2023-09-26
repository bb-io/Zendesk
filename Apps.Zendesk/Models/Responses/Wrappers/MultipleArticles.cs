namespace Apps.Zendesk.Models.Responses.Wrappers;

public class MultipleArticles : PaginatedResponse
{
    public IEnumerable<Article> Articles { get; set; }
}