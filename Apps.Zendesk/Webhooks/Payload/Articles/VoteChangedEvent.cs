namespace Apps.Zendesk.Webhooks.Payload.Articles
{
    public class VoteChangedEvent
    {
        public Current Current { get; set; }
    }

    public class Current
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public int Value { get; set; }
    }
}
