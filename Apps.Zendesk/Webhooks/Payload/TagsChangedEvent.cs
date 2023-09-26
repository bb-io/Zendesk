namespace Apps.Zendesk.Webhooks.Payload;

public class TagsChangedEvent
{
    public Added Added { get; set; }
    public Removed Removed { get; set; }
}

public class Added
{
    public List<string> Tags { get; set; }
}

public class Removed
{
    public List<string> Tags { get; set; }
}