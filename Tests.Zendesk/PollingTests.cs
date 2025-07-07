using Apps.Zendesk.Polling.Models.Memory;
using Blackbird.Applications.Sdk.Common.Polling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZendeskTests.Base;

namespace Tests.Zendesk
{
    [TestClass]
    public class PollingTests :TestBase
    {
        [TestMethod]
        public async Task PollingList_OnLabelsAddedToArticles_works()
        {
            var pollingList = new Apps.Zendesk.Polling.PollingList(InvocationContext);
            var request = new PollingEventRequest<DateMemory>
            {
                Memory = new DateMemory
                {
                    LastInteractionDate = new DateTime(2025, 6, 26, 13, 20, 11, DateTimeKind.Utc)
                }
            };
            var response = await pollingList.OnTicketsAddedToArticles(request);
            var json = System.Text.Json.JsonSerializer.Serialize(response, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);
            Assert.IsNotNull(response);
        }
    }
}
