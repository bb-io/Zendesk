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

    public bool? Public { get; set; }

    public string? Subject { get; set; }

    public object Convert()
    {
        var hasComment = !string.IsNullOrWhiteSpace(Comment);

        if (hasComment)
        {
            return new
            {
                ticket = new
                {
                    comment = new
                    {
                        body = $"{Comment}",
                        @public = Public ?? false
                    },
                    priority = Priority,
                    subject = Subject,
                    status = Status
                }
            };
        }

        return new
        {
            ticket = new
            {
                priority = Priority,
                subject = Subject,
                status = Status
            }
        };
    }
}