using Apps.Zendesk.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.Zendesk.Connections;

public class OAuth2ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups =>
    [
        new ConnectionPropertyGroup
        {
            Name = "OAuth2",
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties =
            [
                new ConnectionProperty(CredNames.BaseUrl)
                {
                    DisplayName = "Base URL"
                }
            ]
        },
    ];

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(Dictionary<string, string> values)
    {
        var token = values.First(v => v.Key == "access_token");
        yield return new AuthenticationCredentialsProvider(CredNames.AccessToken, $"Bearer {token.Value}");

        var url = new Uri(values.First(v => v.Key == CredNames.BaseUrl).Value).GetLeftPart(UriPartial.Authority);
        yield return new AuthenticationCredentialsProvider(CredNames.BaseUrl, url);
    }
}