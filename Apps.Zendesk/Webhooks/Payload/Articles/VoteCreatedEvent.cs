namespace Apps.Zendesk.Webhooks.Payload.Articles;

public class VoteCreatedEvent
{
    public Vote Vote { get; set; }
}

public class Vote
{
    public string Id { get; set; }
    public string UserId { get; set; }
    public int Value { get; set; }
}