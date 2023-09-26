using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Zendesk;

public class ZendeskRequest : RestRequest
{
    public ZendeskRequest(string endpoint, Method method, IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders) : base(endpoint, method)
    {
        this.AddHeader("Authorization", authenticationCredentialsProviders.First(p => p.KeyName == "Authorization").Value);
        this.AddHeader("accept", "*/*");
    }

    public void AddNewtonJson(object obj)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        this.AddJsonBody(json);
    }

    public static ZendeskRequest CreateTranslationUpsertRequest(bool isLocaleMissing, string identifierPart, string? locale, IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        return isLocaleMissing
            ? new ZendeskRequest($"/api/v2/help_center/{identifierPart}/translations", Method.Post, authenticationCredentialsProviders)
            : new ZendeskRequest($"/api/v2/help_center/{identifierPart}/translations/{locale}", Method.Put, authenticationCredentialsProviders);
    }
}