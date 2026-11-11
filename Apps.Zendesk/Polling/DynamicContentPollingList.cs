using System.Security.Cryptography;
using System.Text;
using Apps.Zendesk.Models.DynamicContent;
using Apps.Zendesk.Polling.Models;
using Apps.Zendesk.Polling.Models.Memory;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using RestSharp;

namespace Apps.Zendesk.Polling;

[PollingEventList("Dynamic content")]
public class DynamicContentPollingList(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    private const int MaxEventsPerPoll = 100;
    private const string CreatedEvent = "created";
    private const string UpdatedEvent = "updated";

    private readonly ZendeskClient _client = new(invocationContext);

    [PollingEvent("On dynamic content created or updated",
        "Triggered when a dynamic content item is created or its selected source content changes."),
     MultipleEvents]
    public async Task<PollingEventResponse<DynamicContentPollingMemory, List<DynamicContentChangedEvent>>>
        OnDynamicContentCreatedOrUpdated(
            PollingEventRequest<DynamicContentPollingMemory> request,
            [PollingEventParameter] DynamicContentPollingInput input)
    {
        // Fetch the complete state before computing new memory. If either request fails,
        // this method throws and Blackbird keeps the previously persisted memory.
        var items = await GetAllItems();
        var locales = await GetSupportLocales();

        return BuildResponse(request.Memory, items, locales, input);
    }

    internal static PollingEventResponse<DynamicContentPollingMemory, List<DynamicContentChangedEvent>> BuildResponse(
        DynamicContentPollingMemory? memory,
        IReadOnlyCollection<DynamicContentItem> items,
        IReadOnlyCollection<SupportLocale> locales,
        DynamicContentPollingInput input)
    {
        var requestedLocale = ResolveRequestedLocale(locales, input.SourceLanguage);
        var currentEntries = items
            .Select(item => BuildCurrentEntry(item, locales, requestedLocale))
            .ToDictionary(x => x.Item.Id, StringComparer.Ordinal);
        var currentStates = currentEntries.ToDictionary(
            x => x.Key,
            x => x.Value.State,
            StringComparer.Ordinal);

        if (memory is null || !memory.Initialized)
        {
            return new()
            {
                FlyBird = false,
                Result = null,
                Memory = new DynamicContentPollingMemory
                {
                    Initialized = true,
                    Items = currentStates,
                    PendingChanges = []
                }
            };
        }

        var pending = (memory.PendingChanges ?? [])
            .Where(x => !string.IsNullOrWhiteSpace(x.DynamicContentId))
            .Where(x => currentEntries.ContainsKey(x.DynamicContentId))
            .DistinctBy(x => x.DynamicContentId, StringComparer.Ordinal)
            .ToList();
        var pendingIds = pending
            .Select(x => x.DynamicContentId)
            .ToHashSet(StringComparer.Ordinal);
        var previousStates = memory.Items ?? new Dictionary<string, DynamicContentSourceState>();

        foreach (var entry in currentEntries.Values
                     .OrderBy(x => x.Item.UpdatedAt)
                     .ThenBy(x => x.Item.Id, StringComparer.Ordinal))
        {
            var existed = previousStates.TryGetValue(entry.Item.Id, out var previousState);
            var changed = !existed || !SourceStateEquals(previousState!, entry.State);
            if (!changed || !entry.IsEligible || !MatchesName(entry.Item.Name, input.NameFilter))
                continue;

            if (pendingIds.Add(entry.Item.Id))
            {
                pending.Add(new DynamicContentPendingChange
                {
                    DynamicContentId = entry.Item.Id,
                    EventType = existed ? UpdatedEvent : CreatedEvent
                });
            }
        }

        var emittedDescriptors = pending
            .Where(change => currentEntries[change.DynamicContentId].IsEligible)
            .Take(MaxEventsPerPoll)
            .ToList();
        var emittedEvents = emittedDescriptors
            .Select(change => ToEvent(currentEntries[change.DynamicContentId], change.EventType))
            .ToList();
        var emittedIds = emittedDescriptors
            .Select(x => x.DynamicContentId)
            .ToHashSet(StringComparer.Ordinal);

        return new()
        {
            FlyBird = emittedEvents.Count > 0,
            Result = emittedEvents.Count > 0 ? emittedEvents : null,
            Memory = new DynamicContentPollingMemory
            {
                Initialized = true,
                Items = currentStates,
                PendingChanges = pending
                    .Where(x => !emittedIds.Contains(x.DynamicContentId))
                    .ToList()
            }
        };
    }

    private async Task<List<DynamicContentItem>> GetAllItems()
    {
        var items = new List<DynamicContentItem>();
        string? nextPage = "/api/v2/dynamic_content/items?page[size]=100";
        var visitedPages = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        while (!string.IsNullOrWhiteSpace(nextPage))
        {
            if (!visitedPages.Add(nextPage))
            {
                throw new PluginApplicationException(
                    "Zendesk returned a repeated pagination URL while polling dynamic content.");
            }

            var response = await _client.ExecuteWithHandling<DynamicContentItemsResponse>(
                new ZendeskRequest(nextPage, Method.Get));
            items.AddRange(response.Items);
            nextPage = response.NextPage;
        }

        return items;
    }

    private async Task<List<SupportLocale>> GetSupportLocales()
    {
        var response = await _client.ExecuteWithHandling<SupportLocalesResponse>(
            new ZendeskRequest("/api/v2/locales", Method.Get));
        return response.Locales;
    }

    private static SupportLocale? ResolveRequestedLocale(
        IReadOnlyCollection<SupportLocale> locales,
        string? sourceLanguage)
    {
        if (string.IsNullOrWhiteSpace(sourceLanguage))
            return null;

        var requested = locales.FirstOrDefault(x => string.Equals(
            x.LocaleCode, sourceLanguage.Trim(), StringComparison.OrdinalIgnoreCase));
        if (requested is not null)
            return requested;

        var available = string.Join(", ", locales.Select(x => x.LocaleCode));
        throw new PluginMisconfigurationException(
            $"Source language '{sourceLanguage}' is not enabled in Zendesk Support. Available languages: {available}.");
    }

    private static CurrentEntry BuildCurrentEntry(
        DynamicContentItem item,
        IReadOnlyCollection<SupportLocale> locales,
        SupportLocale? requestedLocale)
    {
        var selectedLocale = requestedLocale;
        DynamicContentVariant? selectedVariant;

        if (requestedLocale is null)
        {
            selectedVariant = item.Variants.FirstOrDefault(x => x.LocaleId == item.DefaultLocaleId)
                ?? item.Variants.FirstOrDefault(x => x.Default);
            var localeId = selectedVariant?.LocaleId ?? item.DefaultLocaleId;
            selectedLocale = locales.FirstOrDefault(x => x.Id == localeId);
        }
        else
        {
            selectedVariant = item.Variants.FirstOrDefault(x => x.LocaleId == requestedLocale.Id);
        }

        if (selectedVariant is not null && selectedLocale is null)
        {
            throw new PluginApplicationException(
                $"Zendesk locale ID '{selectedVariant.LocaleId}' for dynamic content item '{item.Id}' was not returned by the Support Locales API.");
        }

        return new CurrentEntry(
            item,
            selectedVariant,
            selectedLocale?.LocaleCode,
            new DynamicContentSourceState
            {
                SourceLanguage = selectedVariant is null ? null : selectedLocale?.LocaleCode,
                VariantId = selectedVariant?.Id,
                TextHash = selectedVariant is null ? null : HashText(selectedVariant.Content)
            });
    }

    private static bool SourceStateEquals(
        DynamicContentSourceState previous,
        DynamicContentSourceState current) =>
        string.Equals(previous.SourceLanguage, current.SourceLanguage, StringComparison.OrdinalIgnoreCase)
        && string.Equals(previous.VariantId, current.VariantId, StringComparison.Ordinal)
        && string.Equals(previous.TextHash, current.TextHash, StringComparison.Ordinal);

    private static bool MatchesName(string name, string? nameFilter) =>
        string.IsNullOrWhiteSpace(nameFilter)
        || name.Contains(nameFilter.Trim(), StringComparison.OrdinalIgnoreCase);

    private static string HashText(string text) =>
        Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text)));

    private static DynamicContentChangedEvent ToEvent(CurrentEntry entry, string eventType) => new()
    {
        DynamicContentId = entry.Item.Id,
        Name = entry.Item.Name,
        Placeholder = entry.Item.Placeholder,
        Item = entry.Item,
        Variant = entry.Variant!,
        SourceLanguage = entry.SourceLanguage!,
        VariantId = entry.Variant!.Id,
        Text = entry.Variant.Content,
        EventType = eventType
    };

    private sealed record CurrentEntry(
        DynamicContentItem Item,
        DynamicContentVariant? Variant,
        string? SourceLanguage,
        DynamicContentSourceState State)
    {
        public bool IsEligible => Variant is not null && SourceLanguage is not null;
    }
}
