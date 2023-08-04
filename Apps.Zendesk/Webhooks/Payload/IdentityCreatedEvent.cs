namespace Apps.Zendesk.Webhooks.Payload
{
    public class IdentityCreatedEvent
    {
        public IdentityObj Identity { get; set; }
    }

    public class IdentityObj
    {
        public string Id { get; set; }
        public bool Primary { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
