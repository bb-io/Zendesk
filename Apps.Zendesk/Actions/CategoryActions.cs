using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using Apps.Zendesk.Models.Requests;
using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using RestSharp;
using Apps.Zendesk.Models.Identifiers;
using Apps.Zendesk.Models.Responses.Wrappers;

namespace Apps.Zendesk.Actions;

[ActionList("Categories")]
public class CategoryActions(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    private ZendeskClient Client { get; } = new(invocationContext);


    [Action("Search categories", Description = "Search categories, optionally those that are missing translations")]
    public async Task<List<Category>> GetAllCategories([ActionParameter] OptionalMissingLocaleIdentifier missingLocalesInput)
    {
        var endpoint = $"/api/v2/help_center/categories";
        var request = new ZendeskRequest(endpoint, Method.Get);
        var categories = (await Client.GetPaginated<MultipleCategories>(request)).SelectMany(x => x.Categories);

        if (missingLocalesInput.Locales != null && missingLocalesInput.Locales.Any())
        {
            var filteredCategories = new List<Category>();
            foreach (var category in categories)
            {
                var missingLocales = await GetCategoryMissingTranslations(new CategoryIdentifier { Id = category.Id });
                if (missingLocales.Locales.Intersect(missingLocalesInput.Locales).Any())
                    filteredCategories.Add(category);
            }
            categories = filteredCategories;
        }

        return categories.ToList();
    }

    [Action("Get category", Description = "Get information on a specific category")]
    public async Task<Category> GetCategory([ActionParameter] CategoryIdentifier category)
    {
        var endpoint = $"/api/v2/help_center/categories/{category.Id}";
        var request = new ZendeskRequest(endpoint, Method.Get);
        var response = await Client.ExecuteWithHandling<SingleCategory>(request);

        var missingTranslations = await GetCategoryMissingTranslations(category);
        response.Category.MissingLocales = missingTranslations.Locales;

        return response.Category;
    }

    [Action("Create category", Description = "Create a new category")]
    public async Task<Category> CreateCategory([ActionParameter] LocaleIdentifier locale, [ActionParameter] CategoryRequest input)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/categories", Method.Post);
        request.AddNewtonJson(new { category = input });
        var response = await Client.ExecuteWithHandling<SingleCategory>(request);
        return response.Category;
    }

    [Action("Update category", Description = "Update a category")]
    public async Task<Category> UpdateSection([ActionParameter] CategoryIdentifier category, [ActionParameter] CategoryRequest input)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/categories/{category.Id}", Method.Put);
        request.AddNewtonJson(new { category = input });
        var response = await Client.ExecuteWithHandling<SingleCategory>(request);
        return response.Category;
    }

    [Action("Delete category", Description = "Delete a category")]
    public async Task DeleteCategory([ActionParameter] CategoryIdentifier category)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/categories/{category.Id}", Method.Delete);
        await Client.ExecuteWithHandling(request);
    }

    [Action("Get category translation", Description = "Get the translation of a category for a specific locale")]
    public async Task<Translation> GetCategoryTranslation([ActionParameter] CategoryIdentifier category, [ActionParameter] LocaleIdentifier locale)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/categories/{category.Id}/translations/{locale.Locale}", Method.Get);
        var response = await Client.ExecuteWithHandling<SingleTranslation>(request);
        return response.Translation;
    }

    [Action("Get category missing translations", Description = "Get the missing translations of a category")]
    public async Task<MissingLocales> GetCategoryMissingTranslations([ActionParameter] CategoryIdentifier category)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/categories/{category.Id}/translations/missing", Method.Get);
        return await Client.ExecuteWithHandling<MissingLocales>(request);
    }

    [Action("Update category translation", Description = "Update a category translation or create it if it does not exist")]
    public async Task<Translation> TranslateCategory([ActionParameter] CategoryIdentifier category, [ActionParameter] TranslationRequest input)
    {
        var missingLocales = await GetCategoryMissingTranslations(category);
        var isLocaleMissing = missingLocales.Locales.Contains(input.Locale);
        var request = ZendeskRequest.CreateTranslationUpsertRequest(isLocaleMissing, $"categories/{category.Id}", input.Locale);
        request.AddNewtonJson(input.Convert(isLocaleMissing));
        var response = await Client.ExecuteWithHandling<SingleTranslation>(request);
        return response.Translation;
    }


}
