using Apps.Zendesk.Models.Identifiers;
using Apps.Zendesk.Models.Requests;
using Apps.Zendesk.Models.Responses.Wrappers;
using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using RestSharp;

namespace Apps.Zendesk.Actions;

[ActionList]
public class SectionActions : BaseInvocable
{
    private ZendeskClient Client { get; }

    public SectionActions(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new ZendeskClient(invocationContext);
    }

    [Action("Get all sections", Description = "Get all sections, optionally those that are missing translations")]
    public async Task<List<Section>> GetAllSections([ActionParameter] OptionalMissingLocaleIdentifier missingLocalesInput, [ActionParameter] OptionalCategoryIdentifier category)
    {
        var endpoint = category.Id == null ? $"/api/v2/help_center/sections" : $"/api/v2/help_center/categories/{category.Id}/sections";
        var request = new ZendeskRequest(endpoint, Method.Get);
        var sections = (await Client.GetPaginated<MultipleSections>(request)).SelectMany(x => x.Sections);

        if (missingLocalesInput.Locale != null)
        {
            var filteredSections = new List<Section>();
            foreach (var section in sections)
            {
                var missingLocales = await GetSectionMissingTranslations(new SectionIdentifier { Id = section.Id });
                if (missingLocales.Locales.Contains(missingLocalesInput.Locale))
                    filteredSections.Add(section);
            }
            sections = filteredSections;
        }

        return sections.ToList();
    }

    [Action("Get section", Description = "Get information on a specific section")]
    public async Task<Section> GetSection([ActionParameter] SectionIdentifier section)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/sections/{section.Id}", Method.Get);
        var response = await Client.ExecuteWithHandling<SingleSection>(request);
        return response.Section;
    }

    [Action("Create section", Description = "Create a new section")]
    public async Task<Section> CreateSection([ActionParameter] LocaleIdentifier locale, [ActionParameter] CategoryIdentifier category, [ActionParameter] SectionRequest input)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/categories/{category.Id}/sections", Method.Post);
        request.AddNewtonJson(new { section = input });
        var response = await Client.ExecuteWithHandling<SingleSection>(request);
        return response.Section;
    }

    [Action("Update section", Description = "Update a section")]
    public async Task<Section> UpdateSection([ActionParameter] SectionIdentifier section, [ActionParameter] SectionRequest input)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/sections/{section.Id}", Method.Put);
        request.AddNewtonJson(new { section = input });
        var response = await Client.ExecuteWithHandling<SingleSection>(request);
        return response.Section;
    }

    [Action("Delete section", Description = "Delete a section")]
    public async Task DeleteSection([ActionParameter] SectionIdentifier section)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/sections/{section.Id}", Method.Delete);
        await Client.ExecuteWithHandling(request);
    }

    [Action("Get all section translations", Description = "Get all existing translations of this section")]
    public async Task<List<Translation>> GetSectionTranslations([ActionParameter] SectionIdentifier section)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/sections/{section.Id}/translations", Method.Get);
        var translations = (await Client.GetPaginated<MultipleTranslations>(request)).SelectMany(x => x.Translations);
        return translations.ToList();
    }

    [Action("Get section translation", Description = "Get the translation of a section for a specific locale")]
    public async Task<Translation> GetSectionTranslation([ActionParameter] SectionIdentifier section, [ActionParameter] LocaleIdentifier locale)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/sections/{section.Id}/translations/{locale.Locale}", Method.Get);
        var response = await Client.ExecuteWithHandling<SingleTranslation>(request);
        return response.Translation;
    }

    [Action("Get section missing translations", Description = "Get the locales that are missing for this section")]
    public async Task<MissingLocales> GetSectionMissingTranslations([ActionParameter] SectionIdentifier section)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/sections/{section.Id}/translations/missing", Method.Get);
        return await Client.ExecuteWithHandling<MissingLocales>(request);
    }

    [Action("Update section translation", Description = "Updates the translation for a section, creates a new translation if there is none")]
    public async Task<Translation> TranslateCategory([ActionParameter] SectionIdentifier section, [ActionParameter] TranslationRequest input)
    {
        var missingLocales = await GetSectionMissingTranslations(section);
        var isLocaleMissing = missingLocales.Locales.Contains(input.Locale);
        var request = ZendeskRequest.CreateTranslationUpsertRequest(isLocaleMissing, $"sections/{section.Id}", input.Locale);
        request.AddNewtonJson(input.Convert(isLocaleMissing));
        var response = await Client.ExecuteWithHandling<SingleTranslation>(request);
        return response.Translation;
    }


}