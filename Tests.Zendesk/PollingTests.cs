using Apps.Zendesk.Polling.Models;
using Apps.Zendesk.Polling.Models.Memory;
using Blackbird.Applications.Sdk.Common.Polling;
using ZendeskTests.Base;

namespace Tests.Zendesk;

[TestClass]
public class PollingTests : TestBase
{
    [TestMethod]
    public async Task PollingList_OnLabelsAddedToArticles_works()
    {
        var pollingList = new Apps.Zendesk.Polling.PollingList(InvocationContext);
        var input = new OnLabelsAddedInput { Labels = ["Loc MT"] };
        var request = new PollingEventRequest<DateMemory>
        {
            Memory = new DateMemory { LastInteractionDate = new DateTime(2026, 04, 11, 9, 0, 0, DateTimeKind.Utc) }
        };
        var response = await pollingList.OnLabelsAddedToArticles(request, input);
        var json = System.Text.Json.JsonSerializer.Serialize(response, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
        Console.WriteLine(json);
        Assert.IsNotNull(response);
    }
}
