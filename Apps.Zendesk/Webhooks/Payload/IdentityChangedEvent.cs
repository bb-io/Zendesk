namespace Apps.Zendesk.Webhooks.Payload
{
    public class IdentityChangedEvent
    {
        public CurrentObj Current { get; set; }
        public PreviousObj Previous { get; set; }
    }
    public class CurrentObj
    {
        public string Id { get; set; }
        public bool Primary { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class PreviousObj
    {
        public string Id { get; set; }
        public bool Primary { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
