namespace Apps.Zendesk.Webhooks.Payload
{
    public class UserEvent
    {
        public User User { get; set; }
    }

    public class User
    {
        public string Id { get; set; }
    }
}
