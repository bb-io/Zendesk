namespace Apps.Zendesk.Models.Responses.Wrappers;

public class MultipleCategories : PaginatedResponse
{
    public IEnumerable<Category> Categories { get; set; }
}