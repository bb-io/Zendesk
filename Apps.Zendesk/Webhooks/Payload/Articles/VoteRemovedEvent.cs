namespace Apps.Zendesk.Webhooks.Payload.Articles
{
    public class VoteRemovedEvent
    {
        public VoteRemoved Vote { get; set; }
    }

    public class VoteRemoved
    {
        public string Id { get; set; }
    }
}
