using Apps.Zendesk.Models.Responses.Wrappers;
using Apps.Zendesk.Models.Responses;
using Apps.Zendesk.Webhooks.Handlers.ArticleHandlers;
using Apps.Zendesk.Webhooks.Handlers.UserHandlers;
using Apps.Zendesk.Webhooks.Input;
using Apps.Zendesk.Webhooks.Payload;
using Apps.Zendesk.Webhooks.Payload.Articles;
using Apps.Zendesk.Webhooks.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using RestSharp;
using Blackbird.Applications.SDK.Blueprints;

namespace Apps.Zendesk.Webhooks;

[WebhookList("Articles")]
public class WebhookList : BaseInvocable
{
    private ZendeskClient Client { get; }

    public WebhookList(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new ZendeskClient(invocationContext);
    }

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

        if (input.Locale != null && input.Locale != data.Event.Locale)
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

        SingleArticle? articleMetadata = null;
        if ((input.OnlyIfSource != null && input.OnlyIfSource.Value) || input.RequiredLabel != null)
        {
            var locale = data.Event.Locale;
            var id = data.Detail.Id;
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{id}", Method.Get);
            var response = await Client.ExecuteWithHandling<SingleArticle>(request);
            articleMetadata = response;

            if (input.OnlyIfSource != null && input.OnlyIfSource.Value)
            {
                if (response.Article.SourceLocale != locale)
                {
                    return new WebhookResponse<ArticlePublishedResponse>
                    {
                        HttpResponseMessage = null,
                        ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                        Result = null
                    };
                }
            }

            if (input.RequiredLabel != null)
            {
                if (response.Article.Labels == null ||
                    !response.Article.Labels.Contains(input.RequiredLabel, StringComparer.OrdinalIgnoreCase))
                {
                    return new WebhookResponse<ArticlePublishedResponse>
                    {
                        HttpResponseMessage = null,
                        ReceivedWebhookRequestType = WebhookRequestType.Preflight,
                        Result = null
                    };
                }
            }
        }

        return new WebhookResponse<ArticlePublishedResponse>
        {
            HttpResponseMessage = null,
            Result = new ArticlePublishedResponse
            {
                ContentId = data.Detail.Id,
                AuthorId = data.Event.AuthorId,
                CategoryId = data.Event.CategoryId,
                Locale = data.Event.Locale,
                SectionId = data.Event.SectionId,
                Title = data.Event.Title,
                BrandId = data.Detail.BrandId,
                AccountId = data.AccountId.ToString(),
                Labels = articleMetadata?.Article.Labels?.ToList() ?? new List<string>()
            }
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

        var id = data.Detail.Id;
        var request = new ZendeskRequest($"/api/v2/help_center/articles/{id}", Method.Get);
        var response = await Client.ExecuteWithHandling<SingleArticle>(request);
        var article = response.Article; 

        if (input.RequiredLabel != null)
        {
            if (article.Labels == null || !article.Labels.Contains(input.RequiredLabel, StringComparer.OrdinalIgnoreCase))
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
            Result = new ArticlePublishedResponse
            {
                ContentId = data.Detail.Id,
                AuthorId = article.AuthorId,   
                Locale = article.SourceLocale,    
                SectionId = article.SectionId,   
                Title = article.Title,       
                BrandId = data.Detail.BrandId,
                AccountId = data.AccountId.ToString(),
                Labels = article.Labels?.ToList() ?? new List<string>()
            }
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