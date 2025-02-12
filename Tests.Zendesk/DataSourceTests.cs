using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Zendesk.Actions;
using Apps.Zendesk.DataSourceHandlers;
using Apps.Zendesk.Models.Requests;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using ZendeskTests.Base;

namespace Tests.Zendesk
{
    [TestClass]
    public class DataSourceTests : TestBase
    {

        [TestMethod]
        public async Task ArticleDataHandler_works()
        {
            var handler = new ArticleDataHandler(InvocationContext);

            var result = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);

            Console.WriteLine($"Total: {result.Count()}");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Value}: {item.DisplayName}");
            }

            Assert.IsTrue(result.Count() > 0);
        }

    }
}
