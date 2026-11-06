using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;
using Newtonsoft.Json;

namespace Apps.Zendesk.Models;

public class DynamicItem
{
    [JsonProperty("id"), Display("Dynamic content ID")] public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Placeholder { get; set; } = "";
    [JsonProperty("default_locale_id"), Display("Default locale ID")] public long DefaultLocaleId { get; set; }
    [Display("Default language")] public string? DefaultLanguage { get; set; }
    [JsonProperty("created_at")] public DateTime? CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime? UpdatedAt { get; set; }
    public bool Outdated { get; set; }
    public List<DynamicVariant> Variants { get; set; } = new();
}
public class DynamicVariant
{
    [JsonProperty("id"), Display("Variant ID")] public string Id { get; set; } = "";
    [JsonProperty("locale_id"), Display("Locale ID")] public long LocaleId { get; set; }
    [Display("Language")] public string? Language { get; set; }
    public string Content { get; set; } = "";
    public bool Active { get; set; }
    [JsonProperty("default")] public bool Default { get; set; }
    public bool Outdated { get; set; }
    [JsonProperty("created_at")] public DateTime? CreatedAt { get; set; }
    [JsonProperty("updated_at")] public DateTime? UpdatedAt { get; set; }
}
public class SupportLocale { public long Id { get; set; } [JsonProperty("locale")] public string Code { get; set; } = ""; public string Name { get; set; } = ""; }
public class DynamicPage { public List<DynamicItem> Items { get; set; } = new(); public CursorMeta? Meta { get; set; } public CursorLinks? Links { get; set; } }
public class CursorMeta { [JsonProperty("has_more")] public bool HasMore { get; set; } }
public class CursorLinks { public string? Next { get; set; } }
public class SingleDynamicItem { public DynamicItem Item { get; set; } = new(); }
public class SingleDynamicVariant { public DynamicVariant Variant { get; set; } = new(); }
public class SupportLocales { public List<SupportLocale> Locales { get; set; } = new(); }
public class DynamicSearchInput { [Display("Query")] public string? Query { get; set; } }
public class DynamicFindInput { [Display("Dynamic content ID"), DataSource(typeof(DynamicContentDataHandler))] public string? Id { get; set; } [Display("Placeholder")] public string? Placeholder { get; set; } }
public class DynamicIdInput { [Display("Dynamic content ID"), DataSource(typeof(DynamicContentDataHandler))] public string Id { get; set; } = ""; }
public class DynamicLanguageInput { [Display("Source language"), DataSource(typeof(SupportLanguageDataHandler))] public string? Language { get; set; } }
public class DynamicDownloadInput : DynamicLanguageInput { [Display("Content format"), StaticDataSource(typeof(DynamicFormatDataHandler))] public string? Format { get; set; } }
public class DynamicUploadInput
{
    [Display("Content")] public FileReference Content { get; set; } = null!;
    [Display("Target language"), DataSource(typeof(SupportLanguageDataHandler))] public string? Language { get; set; }
    [Display("Dynamic content ID"), DataSource(typeof(DynamicContentDataHandler))] public string? Id { get; set; }
    [Display("Active")] public bool? Active { get; set; }
}
public class DynamicSearchOutput { public List<DynamicItem> Items { get; set; } = new(); [Display("Total count")] public int TotalCount { get; set; } }
public class DynamicFindOutput { public bool Found { get; set; } public DynamicItem? Item { get; set; } }
public class DynamicDownloadOutput { public FileReference Content { get; set; } = null!; public DynamicItem Item { get; set; } = new(); public DynamicVariant Variant { get; set; } = new(); }
public class DynamicUploadOutput { public FileReference Content { get; set; } = null!; [Display("Dynamic content ID")] public string Id { get; set; } = ""; [Display("Variant ID")] public string VariantId { get; set; } = ""; [Display("Target language")] public string Language { get; set; } = ""; public string Operation { get; set; } = ""; }
