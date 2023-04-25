using Apps.Zendesk.Webhooks.Handlers;
using Apps.Zendesk.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;
using System.Text.Json;

namespace Apps.Zendesk.Webhooks
{
    [WebhookList]
    public class WebhookList 
    {
        [Webhook(typeof(UserAliasChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserAliasChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserCreatedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserCreatedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserCustomFieldChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<CustomFieldChangedEvent>>> UserCustomFieldChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<CustomFieldChangedEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<CustomFieldChangedEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserCustomRoleChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserCustomRoleChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserDefaultGroupChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserDefaultGroupChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserDetailsChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserDetailsChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserExternalIDChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserExternalIDChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserGroupMembershipCreatedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<GroupEvent>>> UserGroupMembershipCreatedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<GroupEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<GroupEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserGroupMembershipDeletedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<GroupEvent>>> UserGroupMembershipDeletedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<GroupEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<GroupEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserIdentityChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<IdentityChangedEvent>>> UserIdentityChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<IdentityChangedEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<IdentityChangedEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserIdentityCreatedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<IdentityCreatedEvent>>> UserIdentityCreatedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<IdentityCreatedEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<IdentityCreatedEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserIdentityDeletedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<IdentityCreatedEvent>>> UserIdentityDeletedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<IdentityCreatedEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<IdentityCreatedEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserActiveStatusChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleBooleanEvent>>> UserActiveStatusChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleBooleanEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleBooleanEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserLastLoginChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserLastLoginChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserMergedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<UserEvent>>> UserMergedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<UserEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<UserEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserNameChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserNameChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserNotesChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserNotesChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserOnlyPrivateCommentsStatusChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleBooleanEvent>>> UserOnlyPrivateCommentsStatusChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleBooleanEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleBooleanEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserOrganizationMembershipCreatedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<OrganizationEvent>>> UserOrganizationMembershipCreatedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<OrganizationEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<OrganizationEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserOrganizationMembershipDeletedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<OrganizationEvent>>> UserOrganizationMembershipDeletedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<OrganizationEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<OrganizationEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserPasswordChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<EmptyEvent>>> UserPasswordChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<EmptyEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<EmptyEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserPhotoChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserPhotoChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserRoleChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserRoleChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserDeletedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<EmptyEvent>>> UserDeletedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<EmptyEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<EmptyEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserSuspendedStatusChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleBooleanEvent>>> UserSuspendedStatusChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleBooleanEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleBooleanEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserTagsChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<TagsChangedEvent>>> UserTagsChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<TagsChangedEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<TagsChangedEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

        [Webhook(typeof(UserTimeZoneChangedHandler))]
        public async Task<WebhookResponse<PayloadTemplate<SimpleStringEvent>>> UserTimeZoneChangedHandler(WebhookRequest webhookRequest)
        {
            var data = JsonConvert.DeserializeObject<PayloadTemplate<SimpleStringEvent>>(webhookRequest.Body.ToString());
            if (data is null) { throw new InvalidCastException(nameof(webhookRequest.Body)); }
            return new WebhookResponse<PayloadTemplate<SimpleStringEvent>>
            {
                HttpResponseMessage = null,
                Result = data
            };
        }

    }
}
