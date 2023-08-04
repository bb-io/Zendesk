namespace Apps.Zendesk.Webhooks.Payload
{
    public class CustomFieldChangedEvent
    {
        public Current Current { get; set; }
        public CustomField CustomField { get; set; }
        public Previous Previous { get; set; }
    }

    public class Current
    {
        public string RelationshipTarget { get; set; }
        public string Id { get; set; }
    }

    public class CustomField
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
    }

    public class Previous
    {
        public string RelationshipTarget { get; set; }
        public string Id { get; set; }
    }
}
