namespace Apps.Zendesk.Constants;

public class ConnectionTypes
{
    public const string OAuth2 = "OAuth2";
    
    public const string ApiToken = "API token";
    
    public static readonly IEnumerable<string> SupportedConnectionTypes = [OAuth2, ApiToken];
}