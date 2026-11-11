namespace Apps.Zendesk.Polling.Models.Memory;

public class DynamicContentPollingMemory
{
    public bool Initialized { get; set; }

    public Dictionary<string, DynamicContentSourceState> Items { get; set; } = new();

    public List<DynamicContentPendingChange> PendingChanges { get; set; } = [];
}

public class DynamicContentSourceState
{
    public string? SourceLanguage { get; set; }

    public string? VariantId { get; set; }

    public string? TextHash { get; set; }
}

public class DynamicContentPendingChange
{
    public string DynamicContentId { get; set; } = string.Empty;

    public string EventType { get; set; } = string.Empty;
}
