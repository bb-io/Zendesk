using Apps.Zendesk.Actions;
using Apps.Zendesk.Models.Identifiers;
using ZendeskTests.Base;

namespace Tests.Zendesk
{
    [TestClass]
    public class DataSources : TestBase
    {
        [TestMethod]
        public async Task CategoriesReturnsValues()
        {
            var action = new CategoryActions(InvocationContext);

            var input = new OptionalMissingLocaleIdentifier{ };

            var result = action.GetAllCategories(input);

            foreach (var category in result.Result)
            {
                Console.WriteLine($"{category.Name} - {category.Description}");
                Assert.IsNotNull(category);
            }
        }

        [TestMethod]
        public async Task DataHandlerReturnsValues()
        {
            //19612818365457
            var action = new CategoryActions(InvocationContext);

            var input = new CategoryIdentifier {Id= "196128183657890" };

            var result = action.GetCategory(input);

                Console.WriteLine($"{result.Result.Id} - {result.Result.Name}");
                Assert.IsNotNull(result);
        }


    }
}
