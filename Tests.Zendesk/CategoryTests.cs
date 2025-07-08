using Apps.Zendesk.Actions;
using Apps.Zendesk.Models.Identifiers;
using Blackbird.Applications.Sdk.Common.Files;
using Newtonsoft.Json;
using ZendeskTests.Base;

namespace Tests.Zendesk
{
    [TestClass]
    public class CategoryTests : TestBase
    {
        [TestMethod]
        public async Task CategoriesReturnsValues()
        {
            var action = new CategoryActions(InvocationContext);

            var input = new OptionalMissingLocaleIdentifier { };

            var result = action.GetAllCategories(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            foreach (var category in result.Result)
            {                
                Assert.IsNotNull(category);
            }
        }

        [TestMethod]
        public async Task GetCategoryReturnsValues()
        {
            var action = new CategoryActions(InvocationContext);

            var input = new CategoryIdentifier { Id = "11187144610321" };

            var result = await action.GetCategory(input);

            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
            Assert.IsNotNull(result);
        }

    }
}
