using Blackbird.Applications.Sdk.Common;
namespace Apps.Zendesk.Models.Responses;

public class SectionWithMissingLocales : Section
{
    [Display("Missing locales")]
    public List<string> MissingLocales { get; set; }
}