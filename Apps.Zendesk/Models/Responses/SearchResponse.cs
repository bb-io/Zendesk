namespace Apps.Zendesk.Models.Responses
{
    public class SearchResponse<T>
    {
        public IEnumerable<T> Results { get; set; }
    }
}
