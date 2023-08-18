using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Zendesk.Models.Requests;

public class TicketRequest
{
    public string? Comment { get; set; }
    [DataSource(typeof(PriorityDataHandler))]
    public string? Priority { get; set; }

    [DataSource(typeof(StatusDataHandler))]
    public string? Status { get; set; }
    public string? Subject { get; set; }

    public object Convert()
    {
        return new
        {
            ticket = new
            {
                comment = new
                {
                    body = Comment
                },
                priority = Priority,
                subject = Subject,
                status = Status
            }
        };
    }
}