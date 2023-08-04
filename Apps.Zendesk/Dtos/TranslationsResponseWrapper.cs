namespace Apps.Zendesk.Dtos
{
    public class TranslationsResponseWrapper : PaginatedResponse
    {
        public IEnumerable<TranslationDto> Translations { get; set; }
    }
}
