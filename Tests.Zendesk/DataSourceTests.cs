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

        [TestMethod]
        public async Task LocaleDataHandler_works()
        {
            var handler = new LocaleDataHandler(InvocationContext);

            var result = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);

            Console.WriteLine($"Total: {result.Count()}");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Value}: {item.DisplayName}");
            }

            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public async Task UserSegmentDataHandler_works()
        {
            var handler = new UserSegmentDataHandler(InvocationContext);

            var result = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);

            Console.WriteLine($"Total: {result.Count()}");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Value}: {item.Key}");
            }

            Assert.IsTrue(result.Count() > 0);
        }

        [TestMethod]
        public async Task TicketDataHandler_works()
        {
            var handler = new TicketDataHandler(InvocationContext);

            var result = await handler.GetDataAsync(new DataSourceContext { }, CancellationToken.None);

            Console.WriteLine($"Total: {result.Count()}");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Value}: {item.Key}");
            }

            Assert.IsTrue(result.Count() > 0);
        }


        [TestMethod]
        public async Task StatusDataHandler_works()
        {
            var handler = new StatusDataHandler();

            var result =  handler.GetData(new DataSourceContext { SearchString="" });

            Console.WriteLine($"Total: {result.Count()}");
            foreach (var item in result)
            {
                Console.WriteLine($"{item.Value}: {item.Key}");
            }

            Assert.IsTrue(result.Count() > 0);
        }

    }
}
