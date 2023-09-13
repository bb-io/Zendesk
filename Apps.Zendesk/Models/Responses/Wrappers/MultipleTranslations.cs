namespace Apps.Zendesk.Models.Responses.Wrappers
{
    public class MultipleTranslations : PaginatedResponse
    {
        public IEnumerable<Translation> Translations { get; set; }
    }
}
