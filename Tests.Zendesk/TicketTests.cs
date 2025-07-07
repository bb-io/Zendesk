using Apps.Zendesk.Actions;
using Apps.Zendesk.Models.Requests;
using System.Text.Json;
using ZendeskTests.Base;

namespace Tests.Zendesk
{
    [TestClass]
    public class TicketTests : TestBase
    {
        [TestMethod]
        public async Task SearchTickets_works()
        {
            var actions = new TicketActions(InvocationContext);

            var result = await actions.SearchTickets(new SearchTicketsRequest
            {
                //Status = "open",
                //Priority = "normal"
            });
            Console.WriteLine($"Total tickets found: {result.Tickets.Count()}");
            foreach (var ticket in result.Tickets)
            {
                Console.WriteLine($"Ticket ID: {ticket.Id}, Subject: {ticket.Subject}, Status: {ticket.Status}");
            }
            var json = JsonSerializer.Serialize(result.Tickets, new System.Text.Json.JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(json);
            Assert.IsNotNull(result);
        }
    }
}
