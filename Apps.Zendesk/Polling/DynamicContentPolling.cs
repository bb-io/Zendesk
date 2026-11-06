using Apps.Zendesk.Models;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using System.Security.Cryptography;
using System.Text;

namespace Apps.Zendesk.Polling;

public class DynamicPollingInput : DynamicLanguageInput { [Display("Name filter")] public string? Name { get; set; } }
public class DynamicPollingMemory { public Dictionary<string, DynamicFingerprint> Items { get; set; } = new(); }
public class DynamicFingerprint { public string VariantId { get; set; } = ""; public string Language { get; set; } = ""; public string Hash { get; set; } = ""; }
public class DynamicEvent
{
    public DynamicItem Item { get; set; } = new();
    [Display("Dynamic content ID")] public string DynamicContentId { get; set; } = "";
    public string Name { get; set; } = "";
    public string Placeholder { get; set; } = "";
    [Display("Source language")] public string SourceLanguage { get; set; } = "";
    [Display("Source variant ID")] public string SourceVariantId { get; set; } = "";
    [Display("Source text")] public string SourceText { get; set; } = "";
    [Display("Event type")] public string EventType { get; set; } = "";
}
[PollingEventList]
public class DynamicContentPolling(InvocationContext context) : BaseInvocable(context)
{
    [PollingEvent("On dynamic content created or updated", "Triggers when the selected source text is created or changed"), MultipleEvents]
    public async Task<PollingEventResponse<DynamicPollingMemory, List<DynamicEvent>>> Poll(PollingEventRequest<DynamicPollingMemory> request, [PollingEventParameter] DynamicPollingInput input)
    {
        var items = await new DynamicContentApi(new ZendeskClient(InvocationContext)).List();
        return Compare(items, request.Memory, input);
    }
    public static PollingEventResponse<DynamicPollingMemory, List<DynamicEvent>> Compare(IEnumerable<DynamicItem> items, DynamicPollingMemory? previous, DynamicPollingInput input)
    {
        var current = items.Where(x => string.IsNullOrWhiteSpace(input.Name) || x.Name.Contains(input.Name, StringComparison.OrdinalIgnoreCase))
            .Select(x => (Item: x, Variant: string.IsNullOrWhiteSpace(input.Language) ? x.Variants.FirstOrDefault(v => v.Default || v.LocaleId == x.DefaultLocaleId) : x.Variants.FirstOrDefault(v => string.Equals(v.Language, input.Language, StringComparison.OrdinalIgnoreCase))))
            .Where(x => x.Variant != null && !string.IsNullOrWhiteSpace(x.Variant.Language)).OrderBy(x => x.Item.Id, StringComparer.Ordinal).ToList();
        var memory = new DynamicPollingMemory { Items = previous == null ? new() : new(previous.Items) };
        var events = new List<DynamicEvent>();
        foreach (var (item, selected) in current)
        {
            var variant = selected!;
            var fingerprint = new DynamicFingerprint { VariantId = variant.Id, Language = variant.Language!, Hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(variant.Content ?? ""))) };
            var existed = previous?.Items.TryGetValue(item.Id, out _) == true;
            var old = existed ? previous!.Items[item.Id] : null;
            bool changed = old == null || old.VariantId != fingerprint.VariantId || !string.Equals(old.Language, fingerprint.Language, StringComparison.OrdinalIgnoreCase) || old.Hash != fingerprint.Hash;
            if (previous != null && changed)
            {
                if (events.Count == 100) continue;
                events.Add(new DynamicEvent { Item = item, DynamicContentId = item.Id, Name = item.Name, Placeholder = item.Placeholder, SourceLanguage = variant.Language!, SourceVariantId = variant.Id, SourceText = variant.Content ?? "", EventType = existed ? "updated" : "created" });
            }
            memory.Items[item.Id] = fingerprint;
        }
        return new() { FlyBird = events.Count > 0, Result = events.Count > 0 ? events : null, Memory = memory };
    }
}
