﻿namespace Apps.Zendesk.Webhooks.Payload.Articles;

public class PublishEvent
{
    public string AuthorId { get; set; }
    public string CategoryId { get; set; }
    public string Locale { get; set; }
    public string SectionId { get; set; }
    public string Title { get; set; }
}