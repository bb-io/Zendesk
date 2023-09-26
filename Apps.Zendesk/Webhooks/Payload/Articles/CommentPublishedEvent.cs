namespace Apps.Zendesk.Webhooks.Payload.Articles;

public class CommentPublishedEvent
{
    public CommentPublished Comment { get; set; }
}

public class CommentPublished
{
    public string Id { get; set; }
    public string Locale { get; set; }
}