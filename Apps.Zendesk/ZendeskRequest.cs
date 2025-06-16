using Blackbird.Applications.Sdk.Common.Authentication;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Zendesk;

public class ZendeskRequest(string endpoint, Method method) : RestRequest(endpoint, method)
{
    public void AddNewtonJson(object obj)
    {
        var json = JsonConvert.SerializeObject(obj, Formatting.Indented, new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        });
        this.AddJsonBody(json);
    }

    public static ZendeskRequest CreateTranslationUpsertRequest(bool isLocaleMissing, string identifierPart, string? locale)
    {
        return isLocaleMissing
            ? new ZendeskRequest($"/api/v2/help_center/{identifierPart}/translations", Method.Post)
            : new ZendeskRequest($"/api/v2/help_center/{identifierPart}/translations/{locale}", Method.Put);
    }
}