﻿using Apps.Zendesk.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.SDK.Blueprints.Interfaces.CMS;

namespace Apps.Zendesk.Webhooks.Responses;

public class ArticleResponse : IDownloadContentInput
{
    [Display("Article ID")]
    [DataSource(typeof(ArticleDataHandler))]
    public string ContentId { get; set; }
}