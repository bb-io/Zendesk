using Apps.Zendesk.Extensions;
using Apps.Zendesk.Models.Responses.Wrappers;
using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Utils;
using Apps.Zendesk.Webhooks.Handlers.ArticleHandlers;
using Apps.Zendesk.Webhooks.Input;
using Apps.Zendesk.Webhooks.Payload;
using Apps.Zendesk.Webhooks.Payload.Articles;
using Apps.Zendesk.Webhooks.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using RestSharp;
using Blackbird.Applications.SDK.Blueprints;

namespace Apps.Zendesk.Webhooks;

[WebhookList("Articles")]
public class WebhookList(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    private ZendeskClient Client { get; } = new(invocationContext);

    [Webhook("On article author changed", typeof(ArticleAuthorChangedHandler), Description = "On article author changed")]
    public async Task<WebhookResponse<AuthorChangedResponse>> ArticleAuthorChangedHandler(WebhookRequest webhookRequest, [WebhookParameter] ArticlePublishedInputParameter input)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<AuthorEvent>>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        if (input.BrandId != null && input.BrandId == data.Detail.BrandId)
        {
            return new WebhookResponse<AuthorChangedResponse>
            {
                HttpResponseMessage = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                Result = null
            };
        }

        if (input.AccountId != null && input.AccountId == data.AccountId.ToString())
        {
            return new WebhookResponse<AuthorChangedResponse>
            {
                HttpResponseMessage = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                Result = null
            };
        }

         if (input.ArticleId != null && input.ArticleId != data.Detail.Id)
        {
            return new WebhookResponse<AuthorChangedResponse>
            {
                HttpResponseMessage = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                Result = null
            };
        }

        var id = data.Detail.Id;
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{id}", Method.Get);
        var response = await Client.ExecuteWithHandling<SingleArticle>(request);
        var article = response.Article;

        return new WebhookResponse<AuthorChangedResponse>
        {
            HttpResponseMessage = null,
            Result = new AuthorChangedResponse
            {
                ContentId = data.Detail.Id,
                AuthorId = article.AuthorId,
                Title = article.Title,
                Locale = article.SourceLocale,
                SectionId = article.SectionId,
                Labels = article.Labels?.ToList() ?? new List<string>()
            }
        };
    }

    [BlueprintEventDefinition(BlueprintEvent.ContentCreatedOrUpdated)]
    [Webhook("On article published", typeof(ArticlePublishedHandler), Description = "On article published")]
    public async Task<WebhookResponse<ArticlePublishedResponse>> ArticlePublishedHandler(WebhookRequest webhookRequest, 
        [WebhookParameter] ArticlePublishedInputParameter input)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<PublishEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }

        if (input.BrandId.IsMismatchWith(data.Detail.BrandId) || input.AccountId.IsMismatchWith(data.AccountId.ToString()) || 
            input.Locale.IsMismatchWith(data.Event.Locale) || input.ArticleId.IsMismatchWith(data.Detail.Id))
        {
            return WebhookResponses.NoFlight<ArticlePublishedResponse>();
        }

        ArticlePublishedResponse article;
        try
        {
            article = await CreatePublishedArticleResponse(data, data.Event.Locale);
        }
        // The Zendesk event stream is account-wide, but Help Center endpoints are scoped to the brand of the base URL
        // Every brand connection receives this event. Only the owning one can fetch the article,
        // so a 404 here means 'not my brand' and should skip, not throw:
        catch (Exception ex) when (ex.Message.Contains("Error: RecordNotFound"))
        {
            InvocationContext.Logger?.LogWarning(
                $"[ZendeskWebhooks] Skipping article '{data.Detail.Id}' - not found on '{Client.Options.BaseUrl?.Host}' (event brand '{data.Detail.BrandId}')",
                []);
            return WebhookResponses.NoFlight<ArticlePublishedResponse>();
        }

        if (input.OnlyIfSource == true && !string.Equals(article.SourceLocale, data.Event.Locale, StringComparison.OrdinalIgnoreCase))
            return WebhookResponses.NoFlight<ArticlePublishedResponse>();

        if (!string.IsNullOrWhiteSpace(input.RequiredLabel) && !article.Labels.Contains(input.RequiredLabel, StringComparer.OrdinalIgnoreCase))
            return WebhookResponses.NoFlight<ArticlePublishedResponse>();

        return new WebhookResponse<ArticlePublishedResponse>
        {
            HttpResponseMessage = null,
            Result = article,
        };
    }

    private async Task<MissingLocales> GetArticleMissingTranslations(string articleId)
    {
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{articleId}/translations/missing", Method.Get);
        return await Client.ExecuteWithHandling<MissingLocales>(request);
    }

    public async Task<ArticlePublishedResponse> CreatePublishedArticleResponse<T>(ArticlePayloadTemplate<T> data, string? locale = null)
    {
        var request = !string.IsNullOrWhiteSpace(locale)
               ? new ZendeskRequest($"/api/v2/help_center/{locale}/articles/{data.Detail.Id}", Method.Get)
               : new ZendeskRequest($"/api/v2/help_center/articles/{data.Detail.Id}", Method.Get);
        var response = await Client.ExecuteWithHandling<SingleArticle>(request);
        var missingLocales = await GetArticleMissingTranslations(data.Detail.Id);

        return new ArticlePublishedResponse
        {
            ContentId = data.Detail.Id,
            AuthorId = response?.Article.AuthorId,
            Locale = response?.Article.Locale,
            SectionId = response?.Article.SectionId,
            Title = response?.Article.Title,
            BrandId = data.Detail.BrandId,
            AccountId = data.AccountId.ToString(),
            Labels = response?.Article.Labels?.ToList() ?? [],
            OutdatedLocales = response?.Article.OutdatedLocales?.ToList() ?? [],
            MissingLocales = missingLocales.Locales,
            SourceLocale = response?.Article?.SourceLocale,
        };
    }


    [Webhook("On article subscription created", typeof(ArticleSubscriptionCreatedHandler), Description = "On article subscription created")]
    public async Task<WebhookResponse<ArticleSubscriptionCreatedResponse>> ArticleSubscriptionCreatedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<SubscriptionCreatedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<ArticleSubscriptionCreatedResponse>
        {
            HttpResponseMessage = null,
            Result = new ArticleSubscriptionCreatedResponse
            {
                ContentId = data.Detail.Id,
                Id = data.Event.Subscription.Id,
                UserId = data.Event.Subscription.UserId
            }
        };
    }

    [Webhook("On article unpublished", typeof(ArticleUnpublishedHandler), Description = "On article unpublished")]
    public async Task<WebhookResponse<ArticlePublishedResponse>> ArticleUnpublishedHandler(WebhookRequest webhookRequest, [WebhookParameter] ArticlePublishedInputParameter input)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<EmptyEvent>>(webhookRequest.Body.ToString());
        if (data is null)
        {
            throw new InvalidCastException(nameof(webhookRequest.Body));
        }

        if (input.BrandId != null && input.BrandId == data.Detail.BrandId)
        {
            return new WebhookResponse<ArticlePublishedResponse>
            {
                HttpResponseMessage = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                Result = null
            };
        }

        if (input.AccountId != null && input.AccountId == data.AccountId.ToString())
        {
            return new WebhookResponse<ArticlePublishedResponse>
            {
                HttpResponseMessage = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                Result = null
            };
        }

        if (input.ArticleId != null && input.ArticleId != data.Detail.Id)
        {
            return new WebhookResponse<ArticlePublishedResponse>
            {
                HttpResponseMessage = null,
                ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                Result = null
            };
        }

        var article = await CreatePublishedArticleResponse(data);

        if (input.RequiredLabel != null)
        {
            if (!article.Labels.Contains(input.RequiredLabel, StringComparer.OrdinalIgnoreCase))
            {
                return new WebhookResponse<ArticlePublishedResponse>
                {
                    HttpResponseMessage = null,
                    ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                    Result = null
                };
            }
        }

        return new WebhookResponse<ArticlePublishedResponse>
        {
            HttpResponseMessage = null,
            Result = article
        };
    }

    [Webhook("On article vote created", typeof(ArticleVoteCreatedHandler), Description = "On article vote created")]
    public async Task<WebhookResponse<VoteResponse>> ArticleVoteCreatedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<VoteCreatedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<VoteResponse>
        {
            HttpResponseMessage = null,
            Result = new VoteResponse
            {
                ContentId = data.Detail.Id,
                Id = data.Event.Vote.Id,
                UserId = data.Event.Vote.UserId,
                Value = data.Event.Vote.Value,
            }
        };
    }

    [Webhook("On article vote changed", typeof(ArticleVoteChangedHandler), Description = "On article vote changed")]
    public async Task<WebhookResponse<VoteResponse>> ArticleVoteChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<VoteChangedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<VoteResponse>
        {
            HttpResponseMessage = null,
            Result = new VoteResponse
            {
                ContentId = data.Detail.Id,
                Id = data.Event.Current.Id,
                UserId = data.Event.Current.UserId,
                Value = data.Event.Current.Value,
            }
        };
    }

    [Webhook("On article vote removed", typeof(ArticleVoteRemovedHandler), Description = "On article vote removed")]
    public async Task<WebhookResponse<VoteRemovedResponse>> ArticleVoteRemovedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<VoteRemovedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<VoteRemovedResponse>
        {
            HttpResponseMessage = null,
            Result = new VoteRemovedResponse { ContentId = data.Detail.Id, Id = data.Event.Vote.Id }
        };
    }

    [Webhook("On article comment created", typeof(ArticleCommentCreatedHandler), Description = "On article comment created")]
    public async Task<WebhookResponse<CommentCreatedResponse>> ArticleCommentCreatedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<CommentCreatedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<CommentCreatedResponse>
        {
            HttpResponseMessage = null,
            Result = new CommentCreatedResponse { 
                ContentId = data.Detail.Id,
                Id = data.Event.Comment.Id,
                AuthorId = data.Event.Comment.AuthorId,
                Locale = data.Event.Comment.Locale
            }
        };
    }

    [Webhook("On article comment changed", typeof(ArticleCommentChangedHandler), Description = "On article comment changed")]
    public async Task<WebhookResponse<CommentChangedResponse>> ArticleCommentChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<CommentChangedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<CommentChangedResponse>
        {
            HttpResponseMessage = null,
            Result = new CommentChangedResponse
            {
                ContentId = data.Detail.Id,
                Id = data.Event.Current.Id
            }
        };
    }

    [Webhook("On article comment published", typeof(ArticleCommentPublishedHandler), Description = "On article comment published")]
    public async Task<WebhookResponse<CommentPublishResponse>> ArticleCommentPublishedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<CommentPublishedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<CommentPublishResponse>
        {
            HttpResponseMessage = null,
            Result = new CommentPublishResponse
            {
                ContentId = data.Detail.Id,
                Id = data.Event.Comment.Id,
                Locale = data.Event.Comment.Locale
            }
        };
    }

    [Webhook("On article comment unpublished", typeof(ArticleCommentUnpublishedHandler), Description = "On article comment unpublished")]
    public async Task<WebhookResponse<CommentPublishResponse>> ArticleCommentUnpublishedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<CommentUnpublishedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<CommentPublishResponse>
        {
            HttpResponseMessage = null,
            Result = new CommentPublishResponse
            {
                ContentId = data.Detail.Id,
                Id = data.Event.Comment.Id,
                Locale = data.Event.Comment.Locale
            }
        };
    }

}