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
            Name = ConnectionTypes.OAuth2,
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties =
            [
                new ConnectionProperty(CredNames.BaseUrl)
                {
                    DisplayName = "Base URL"
                }
            ]
        },
        new ConnectionPropertyGroup
        {
            Name = ConnectionTypes.ApiToken,
            AuthenticationType = ConnectionAuthenticationType.Undefined,
            ConnectionProperties =
            [
                new ConnectionProperty(CredNames.BaseUrl)
                {
                    DisplayName = "Base URL"
                },
                new ConnectionProperty(CredNames.AccessToken)
                {
                    DisplayName = "API Token",
                    Description = "Zendesk API token. You can create it in your Zendesk account settings.",
                    Sensitive = true
                }
            ]
        }
    ];

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(Dictionary<string, string> values)
    {
        var credentials = values.Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value)).ToList();
        var connectionType = values[nameof(ConnectionPropertyGroup)] switch
        {
            var ct when ConnectionTypes.SupportedConnectionTypes.Contains(ct) => ct,
            _ => throw new Exception($"Unknown connection type: {values[nameof(ConnectionPropertyGroup)]}")
        };
        
        credentials.Add(new AuthenticationCredentialsProvider(CredNames.ConnectionType, connectionType));
        return credentials;
    }
}