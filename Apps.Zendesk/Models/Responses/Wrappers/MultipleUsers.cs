namespace Apps.Zendesk.Models.Responses.Wrappers;

public class MultipleUsers : PaginatedResponse
{
    public IEnumerable<NamedResource> Users { get; set; }
}