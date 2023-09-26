namespace Apps.Zendesk.Webhooks.Payload;

public class OrganizationEvent
{
    public Organization Organization { get; set; }
}

public class Organization
{
    public string Id { get; set; }
}