namespace Apps.Zendesk.Webhooks.Payload
{
    public class GroupEvent
    {
        public Group Group { get; set; }
    }

    public class Group
    {
        public string Id { get; set; }
    }
}
