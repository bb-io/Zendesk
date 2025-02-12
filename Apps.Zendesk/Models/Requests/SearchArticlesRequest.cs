using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Zendesk.Models.Requests;
public class SearchArticlesRequest
{
    [Display("Query", Description = "The search text to be matched or a search string. Examples: \"carrot potato\", \"'carrot potato'\".")]
    public string? Query { get; set; }

    [Display("Category IDs")]
    [DataSource(typeof(CategoryDataHandler))]
    public IEnumerable<string>? CategoryIds { get; set; }

    [Display("Section IDs")]
    [DataSource(typeof(SectionDataHandler))]
    public IEnumerable<string>? SectionIds { get; set; }

    [Display("Label names")]
    public IEnumerable<string>? LabelNames { get; set; }

    [DataSource(typeof(LocaleDataHandler))]
    [Display("Locale")]
    public string? Locale { get; set; }

    [Display("Created after")]
    public DateTime? CreatedAfter { get; set; }

    [Display("Created before")]
    public DateTime? CreatedBefore { get; set; }

    [Display("Created at")]
    public DateTime? CreatedAt { get; set; }

    [Display("Updated after")]
    public DateTime? UpdatedAfter { get; set; }

    [Display("Updated before")]
    public DateTime? UpdatedBefore { get; set; }

    [Display("Updated at")]
    public DateTime? UpdatedAt { get; set; }
}
