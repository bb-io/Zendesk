namespace Apps.Zendesk.Constants;

public class ConnectionTypes
{
    public const string OAuth2 = "OAuth 2.0";
    
    public const string ApiToken = "API token";
    
    public static readonly IEnumerable<string> SupportedConnectionTypes = [OAuth2, ApiToken];
}