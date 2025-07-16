using Apps.Zendesk.Actions;
using Newtonsoft.Json;
using ZendeskTests.Base;

namespace Tests.Zendesk
{
    [TestClass]
    public class GeneralTests:TestBase
    {
        [TestMethod]
        public async Task Get_languages_works()
        {
            var actions = new HelpCenterActions(InvocationContext);
            var result = await actions.ListLocales();
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

            Assert.IsTrue(result.AdditionalLocales.Count() > 0);
        }
    }
}
