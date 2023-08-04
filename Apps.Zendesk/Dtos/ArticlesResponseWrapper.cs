namespace Apps.Zendesk.Dtos
{
    public class ArticlesResponseWrapper : PaginatedResponse
    {
        public IEnumerable<ArticleDto> Articles { get; set; }
    }
}
