using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using Apps.Zendesk.Models.Requests;
using Apps.Zendesk.Models.Responses;
using Blackbird.Applications.Sdk.Common.Actions;
using RestSharp;
using Apps.Zendesk.Models.Identifiers;
using Apps.Zendesk.Models.Responses.Wrappers;

namespace Apps.Zendesk.Actions
{
    [ActionList]
    public class CategoryActions : BaseInvocable
    {
        private IEnumerable<AuthenticationCredentialsProvider> Creds =>
            InvocationContext.AuthenticationCredentialsProviders;

        public CategoryActions(InvocationContext invocationContext) : base(invocationContext)
        {
        }


        [Action("Get all categories", Description = "Get all categories, optionally those that are missing translations")]
        public async Task<List<Category>> GetAllCategories([ActionParameter] OptionalMissingLocaleIdentifier missingLocalesInput)
        {
            var client = new ZendeskClient(Creds);
            var endpoint = $"/api/v2/help_center/categories";
            var request = new ZendeskRequest(endpoint, Method.Get, Creds);
            var categories = client.GetPaginated<MultipleCategories>(request).SelectMany(x => x.Categories);

            if (missingLocalesInput.Locale != null)
            {
                var filteredCategories = new List<Category>();
                foreach (var category in categories)
                {
                    var missingLocales = await GetCategoryMissingTranslations(new CategoryIdentifier { Id = category.Id });
                    if (missingLocales.Locales.Contains(missingLocalesInput.Locale))
                        filteredCategories.Add(category);
                }
                categories = filteredCategories;
            }

            return categories.ToList();
        }

        [Action("Get category", Description = "Get information on a specific category")]
        public async Task<Category> GetCategory([ActionParameter] CategoryIdentifier category)
        {
            var client = new ZendeskClient(Creds);
            var endpoint = $"/api/v2/help_center/categories/{category.Id}";
            var request = new ZendeskRequest(endpoint, Method.Get, Creds);
            var response = await client.ExecuteWithHandling<SingleCategory>(request);
            return response.Category;
        }

        [Action("Get category articles", Description = "Get all articles that belong to a specific category")]
        public async Task<List<Article>> GetArticles([ActionParameter] CategoryIdentifier category)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/categories/{category.Id}/articles", Method.Get, Creds);
            var articles = client.GetPaginated<MultipleArticles>(request).SelectMany(x => x.Articles);
            return articles.ToList();
        }

        [Action("Create category", Description = "Create a new category")]
        public async Task<Category> CreateCategory([ActionParameter] LocaleIdentifier locale, [ActionParameter] CategoryRequest input)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/{locale.Locale}/categories", Method.Post, Creds);
            request.AddNewtonJson(new { category = input });
            var response = await client.ExecuteWithHandling<SingleCategory>(request);
            return response.Category;
        }

        [Action("Update category", Description = "Update a category")]
        public async Task<Category> UpdateSection([ActionParameter] CategoryIdentifier category, [ActionParameter] CategoryRequest input)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/categories/{category.Id}", Method.Put, Creds);
            request.AddNewtonJson(new { category = input });
            var response = await client.ExecuteWithHandling<SingleCategory>(request);
            return response.Category;
        }

        [Action("Delete category", Description = "Delete a category")]
        public async Task DeleteCategory([ActionParameter] CategoryIdentifier category)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/categories/{category.Id}", Method.Delete, Creds);
            await client.ExecuteWithHandling(request);
        }

        [Action("Get all category translations", Description = "Get all existing translations of this category")]
        public async Task<List<Translation>> GetCategoryTranslations([ActionParameter] CategoryIdentifier category)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/categories/{category.Id}/translations", Method.Get, Creds);
            var translations = client.GetPaginated<MultipleTranslations>(request).SelectMany(x => x.Translations);
            return translations.ToList();
        }

        [Action("Get category translation", Description = "Get the translation of a category for a specific locale")]
        public async Task<Translation> GetCategoryTranslation([ActionParameter] CategoryIdentifier category, [ActionParameter] LocaleIdentifier locale)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/categories/{category.Id}/translations/{locale.Locale}", Method.Get, Creds);
            var response = await client.ExecuteWithHandling<SingleTranslation>(request);
            return response.Translation;
        }

        [Action("Get category missing translations", Description = "Get the locales that are missing for this category")]
        public async Task<MissingLocales> GetCategoryMissingTranslations([ActionParameter] CategoryIdentifier category)
        {
            var client = new ZendeskClient(Creds);
            var request = new ZendeskRequest($"/api/v2/help_center/categories/{category.Id}/translations/missing", Method.Get, Creds);
            return await client.ExecuteWithHandling<MissingLocales>(request);
        }

        [Action("Update category translation", Description = "Updates the translation for a category, creates a new translation if there is none")]
        public async Task<Translation> TranslateCategory([ActionParameter] CategoryIdentifier category, [ActionParameter] TranslationRequest input)
        {
            var client = new ZendeskClient(Creds);
            var missingLocales = await GetCategoryMissingTranslations(category);
            var isLocaleMissing = missingLocales.Locales.Contains(input.Locale);
            var request = ZendeskRequest.CreateTranslationUpsertRequest(isLocaleMissing, $"categories/{category.Id}", input.Locale, Creds);
            request.AddNewtonJson(input.Convert(isLocaleMissing));
            var response = await client.ExecuteWithHandling<SingleTranslation>(request);
            return response.Translation;
        }


    }
}
