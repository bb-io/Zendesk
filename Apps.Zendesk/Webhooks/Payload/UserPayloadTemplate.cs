namespace Apps.Zendesk.Webhooks.Payload
{
    public class UserPayloadTemplate<T>
    {
        public int AccountId { get; set; }
        public Detail Detail { get; set; }
        public T Event { get; set; }
        public string Id { get; set; }
        public string Subject { get; set; }
        public string Time { get; set; }
        public string Type { get; set; }
        public string ZendeskEventVersion { get; set; }
    }
    public class Detail
    {
        public string CreatedAt { get; set; }
        public string DefaultGroupId { get; set; }
        public string Email { get; set; }
        public string ExternalId { get; set; }
        public string Id { get; set; }
        public string OrganizationId { get; set; }
        public string Role { get; set; }
        public string UpdatedAt { get; set; }
    }
}
