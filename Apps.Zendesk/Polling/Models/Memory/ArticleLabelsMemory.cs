namespace Apps.Zendesk.Polling.Models.Memory;

public class ArticleLabelsMemory
{
    public DateTime LastInteractionDate { get; set; }
    public Dictionary<string, List<string>> ArticleLabels { get; set; } = new();
}
