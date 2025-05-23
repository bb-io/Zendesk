﻿using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZendeskTests.Base;

namespace Tests.Zendesk
{
    [TestClass]
    public class DataHandlerTests :TestBase
    {
        [TestMethod]
        public async Task LabelNameDataHandler_IsSucces()
        {
            var handler = new LabelNameDataHandler(InvocationContext);

            var result = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

            foreach (var label in result)
            {
                Console.WriteLine($"{label.Value} - {label.Key}");
                Assert.IsNotNull(label);
            }
        }

        [TestMethod]
        public async Task LabelDataHandler_IsSucces()
        {
            var handler = new LabelDataHandler(InvocationContext);

            var result = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

            foreach (var label in result)
            {
                Console.WriteLine($"{label.Value} - {label.Key}");
                Assert.IsNotNull(label);
            }
        }

        [TestMethod]
        public async Task ArticleDataHandler_IsSucces()
        {
            var handler = new ArticleDataHandler(InvocationContext);

            var result = await handler.GetDataAsync(new DataSourceContext(), CancellationToken.None);

            foreach (var label in result)
            {
                Console.WriteLine($"{label.Value} - {label.DisplayName}");
                Assert.IsNotNull(label);
            }
        }
    }
}
