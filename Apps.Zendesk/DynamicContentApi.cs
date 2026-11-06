using Apps.Zendesk.Models;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace Apps.Zendesk;

public class DynamicContentApi(ZendeskClient client)
{
    public async Task<List<SupportLocale>> Locales() => (await client.ExecuteWithHandling<SupportLocales>(new ZendeskRequest("/api/v2/locales", Method.Get))).Locales;
    public async Task<List<DynamicItem>> List()
    {
        var items = new List<DynamicItem>();
        string? next = "/api/v2/dynamic_content/items?page[size]=100";
        var visited = new HashSet<string>();
        while (next != null)
        {
            if (!visited.Add(next)) throw new PluginApplicationException("Zendesk returned a repeated dynamic content page URL.");
            var page = await client.ExecuteWithHandling<DynamicPage>(new ZendeskRequest(next, Method.Get));
            items.AddRange(page.Items);
            if (page.Meta?.HasMore != true) break;
            next = page.Links?.Next ?? throw new PluginApplicationException("Zendesk reported another dynamic content page without a next URL.");
            var accountUri = new Uri(client.BaseUrl);
            if (!Uri.TryCreate(next, UriKind.Absolute, out var uri) || !string.Equals(uri.Authority, accountUri.Authority, StringComparison.OrdinalIgnoreCase) || !string.Equals(uri.Scheme, accountUri.Scheme, StringComparison.OrdinalIgnoreCase))
                throw new PluginApplicationException("Zendesk returned an invalid dynamic content page URL.");
        }
        Enrich(items, await Locales());
        return items;
    }
    public async Task<DynamicItem?> Get(string id)
    {
        if (!long.TryParse(id, out var numericId) || numericId <= 0) throw new PluginMisconfigurationException("Dynamic content ID must be a positive number.");
        var request = new ZendeskRequest($"/api/v2/dynamic_content/items/{numericId}", Method.Get);
        var response = await client.ExecuteWithHandling(request, allowNotFound: true);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            // Invalid endpoints and other 404 errors should still surface through normal handling.
            try { var error = JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content ?? ""); if (error?.TryGetValue("error", out var value) == true && value?.ToString() == "RecordNotFound") return null; } catch (JsonException) { }
        }
        if (!response.IsSuccessStatusCode) throw new PluginApplicationException($"Zendesk returned HTTP {(int)response.StatusCode}: {response.Content}");
        var item = JsonConvert.DeserializeObject<SingleDynamicItem>(response.Content ?? "")?.Item ?? throw new PluginApplicationException("Zendesk returned an invalid dynamic content item.");
        Enrich(new[] { item }, await Locales());
        return item;
    }
    public async Task<DynamicVariant> Save(string id, DynamicVariant? existing, SupportLocale locale, string content, bool? active)
    {
        var request = new ZendeskRequest($"/api/v2/dynamic_content/items/{id}/variants" + (existing == null ? "" : $"/{existing.Id}"), existing == null ? Method.Post : Method.Put);
        request.AddNewtonJson(new { variant = new { locale_id = existing == null ? (long?)locale.Id : null, content, active = active ?? (existing == null ? true : (bool?)null), @default = existing == null ? (bool?)false : null } });
        var result = (await client.ExecuteWithHandling<SingleDynamicVariant>(request)).Variant;
        result.Language = locale.Code;
        return result;
    }
    public static void Enrich(IEnumerable<DynamicItem> items, IEnumerable<SupportLocale> locales)
    {
        var map = locales.ToDictionary(x => x.Id, x => x.Code);
        foreach (var item in items) { item.DefaultLanguage = map.GetValueOrDefault(item.DefaultLocaleId); foreach (var variant in item.Variants) variant.Language = map.GetValueOrDefault(variant.LocaleId); }
    }
    public static string NormalizePlaceholder(string value)
    {
        value = value.Trim();
        if (value.StartsWith("{{") && value.EndsWith("}}")) value = value[2..^2].Trim();
        if (value.StartsWith("dc.", StringComparison.OrdinalIgnoreCase)) value = value[3..];
        if (string.IsNullOrWhiteSpace(value) || value.Contains('{') || value.Contains('}')) throw new PluginMisconfigurationException("Provide a valid dynamic content placeholder.");
        return value;
    }
}
