namespace Apps.Zendesk.Webhooks.Payload.Articles
{
    public class CommentChangedEvent
    {
        public CommentChanged Current { get; set; }
    }

    public class CommentChanged
    {
        public string Id { get; set; }
    }
}
