namespace Apps.Zendesk.Models.Responses.Wrappers
{
    public class MultipleSections : PaginatedResponse
    {
        public IEnumerable<Section> Sections { get; set; }
    }
}
