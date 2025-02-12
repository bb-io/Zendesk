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

namespace Apps.Zendesk.Webhooks;

[WebhookList]
public class WebhookList : BaseInvocable
{
    private ZendeskClient Client { get; }

    public WebhookList(InvocationContext invocationContext) : base(invocationContext)
    {
        Client = new ZendeskClient(invocationContext);
    }

    [Webhook("On user alias changed", typeof(UserAliasChangedHandler), Description = "On user alias changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserAliasChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user created", typeof(UserCreatedHandler), Description = "On user created")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserCreatedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user custom field changed", typeof(UserCustomFieldChangedHandler), Description = "On user custom field changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<CustomFieldChangedEvent>>> UserCustomFieldChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<CustomFieldChangedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<CustomFieldChangedEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user custom role changed", typeof(UserCustomRoleChangedHandler), Description = "On user custom role changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserCustomRoleChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user default group changed", typeof(UserDefaultGroupChangedHandler), Description = "On user default group changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserDefaultGroupChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user details changed", typeof(UserDetailsChangedHandler), Description = "On user details changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserDetailsChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user external ID changed", typeof(UserExternalIDChangedHandler), Description = "On user external ID changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserExternalIDChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user group membership created", typeof(UserGroupMembershipCreatedHandler), Description = "On user group membership created")]
    public async Task<WebhookResponse<UserPayloadTemplate<GroupEvent>>> UserGroupMembershipCreatedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<GroupEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<GroupEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user group membership deleted", typeof(UserGroupMembershipDeletedHandler), Description = "On user group membership deleted")]
    public async Task<WebhookResponse<UserPayloadTemplate<GroupEvent>>> UserGroupMembershipDeletedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<GroupEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<GroupEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user identity changed", typeof(UserIdentityChangedHandler), Description = "On user identity changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<IdentityChangedEvent>>> UserIdentityChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<IdentityChangedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<IdentityChangedEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user identity created", typeof(UserIdentityCreatedHandler), Description = "On user identity created")]
    public async Task<WebhookResponse<UserPayloadTemplate<IdentityCreatedEvent>>> UserIdentityCreatedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<IdentityCreatedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<IdentityCreatedEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user identity deleted", typeof(UserIdentityDeletedHandler), Description = "On user identity deleted")]
    public async Task<WebhookResponse<UserPayloadTemplate<IdentityCreatedEvent>>> UserIdentityDeletedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<IdentityCreatedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<IdentityCreatedEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user active status changed", typeof(UserActiveStatusChangedHandler), Description = "On user active status changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleBooleanEvent>>> UserActiveStatusChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleBooleanEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleBooleanEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user last login changed", typeof(UserLastLoginChangedHandler), Description = "On user last login changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserLastLoginChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user merged", typeof(UserMergedHandler), Description = "On user merged")]
    public async Task<WebhookResponse<UserPayloadTemplate<UserEvent>>> UserMergedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<UserEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<UserEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user name changed", typeof(UserNameChangedHandler), Description = "On user name changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserNameChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user notes changed", typeof(UserNotesChangedHandler), Description =  "On user notes changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserNotesChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user only private comments status changed", typeof(UserOnlyPrivateCommentsStatusChangedHandler), Description = "On user only private comments status changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleBooleanEvent>>> UserOnlyPrivateCommentsStatusChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleBooleanEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleBooleanEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user organization membership created", typeof(UserOrganizationMembershipCreatedHandler), Description = "On user organization membership created")]
    public async Task<WebhookResponse<UserPayloadTemplate<OrganizationEvent>>> UserOrganizationMembershipCreatedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<OrganizationEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<OrganizationEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user organization membership deleted", typeof(UserOrganizationMembershipDeletedHandler), Description = "On user organization membership deleted")]
    public async Task<WebhookResponse<UserPayloadTemplate<OrganizationEvent>>> UserOrganizationMembershipDeletedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<OrganizationEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<OrganizationEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user password changed", typeof(UserPasswordChangedHandler), Description = "On user password changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<EmptyEvent>>> UserPasswordChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<EmptyEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<EmptyEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user photo changed", typeof(UserPhotoChangedHandler), Description = "On user photo changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserPhotoChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user role changed", typeof(UserRoleChangedHandler), Description = "On user role changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserRoleChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user deleted", typeof(UserDeletedHandler), Description = "On user deleted")]
    public async Task<WebhookResponse<UserPayloadTemplate<EmptyEvent>>> UserDeletedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<EmptyEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<EmptyEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user suspended status changed", typeof(UserSuspendedStatusChangedHandler), Description = "On user suspended status changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleBooleanEvent>>> UserSuspendedStatusChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleBooleanEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleBooleanEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user tags changed", typeof(UserTagsChangedHandler), Description = "On user tags changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<TagsChangedEvent>>> UserTagsChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<TagsChangedEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<TagsChangedEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On user time zone changed", typeof(UserTimeZoneChangedHandler), Description = "On user time zone changed")]
    public async Task<WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>> UserTimeZoneChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<UserPayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<UserPayloadTemplate<SimpleStringEvent>>
        {
            HttpResponseMessage = null,
            Result = data
        };
    }

    [Webhook("On article author changed", typeof(ArticleAuthorChangedHandler), Description = "On article author changed")]
    public async Task<WebhookResponse<AuthorChangedResponse>> ArticleAuthorChangedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<AuthorEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<AuthorChangedResponse>
        {
            HttpResponseMessage = null,
            Result = new AuthorChangedResponse
            {
                ArticleId = data.Detail.Id,
                AuthorId = data.Event.Current
            }
        };
    }

    [Webhook("On article published", typeof(ArticlePublishedHandler), Description = "On article published")]
    public async Task<WebhookResponse<ArticlePublishedResponse>> ArticlePublishedHandler(WebhookRequest webhookRequest, [WebhookParameter] ArticlePublishedInputParameter input)
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

        if (input.OnlyIfSource != null && input.OnlyIfSource.Value)
        {
            var locale = data.Event.Locale;
            var id = data.Detail.Id;
            var request = new ZendeskRequest($"/api/v2/help_center/articles/{id}", Method.Get);
            var response = await Client.ExecuteWithHandling<SingleArticle>(request);

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

        return new WebhookResponse<ArticlePublishedResponse>
        {
            HttpResponseMessage = null,
            Result = new ArticlePublishedResponse
            {
                ArticleId = data.Detail.Id,
                AuthorId = data.Event.AuthorId,
                CategoryId = data.Event.CategoryId,
                Locale = data.Event.Locale,
                SectionId = data.Event.SectionId,
                Title = data.Event.Title,
                BrandId = data.Detail.BrandId,
                AccountId = data.AccountId.ToString(),
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
                ArticleId = data.Detail.Id,
                Id = data.Event.Subscription.Id,
                UserId = data.Event.Subscription.UserId
            }
        };
    }

    [Webhook("On article unpublished", typeof(ArticleUnpublishedHandler), Description = "On article unpublished")]
    public async Task<WebhookResponse<ArticleResponse>> ArticleUnpublishedHandler(WebhookRequest webhookRequest)
    {
        var data = JsonConvert.DeserializeObject<ArticlePayloadTemplate<EmptyEvent>>(webhookRequest.Body.ToString());
        if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
        return new WebhookResponse<ArticleResponse>
        {
            HttpResponseMessage = null,
            Result = new ArticleResponse
            {
                ArticleId = data.Detail.Id,
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
                ArticleId = data.Detail.Id,
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
                ArticleId = data.Detail.Id,
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
            Result = new VoteRemovedResponse { ArticleId = data.Detail.Id, Id = data.Event.Vote.Id }
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
                ArticleId = data.Detail.Id,
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
                ArticleId = data.Detail.Id,
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
                ArticleId = data.Detail.Id,
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
                ArticleId = data.Detail.Id,
                Id = data.Event.Comment.Id,
                Locale = data.Event.Comment.Locale
            }
        };
    }

}