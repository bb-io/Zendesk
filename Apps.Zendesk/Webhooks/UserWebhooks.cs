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

//[WebhookList("Users")] Removed until there are useful user actions
public class UserWebhooks : BaseInvocable
{
    private ZendeskClient Client { get; }

    public UserWebhooks(InvocationContext invocationContext) : base(invocationContext)
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

}