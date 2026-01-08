using ZendeskTests.Base;
using Apps.Zendesk.Auth.OAuth2;

namespace Tests.Zendesk;

[TestClass]
public class OAuth2Tests : TestBase
{
    [TestMethod]
    public async Task IsRefreshToken_IsSuccess()
    {
        // Arrange
        var service = new OAuth2TokenService(InvocationContext);
        var values = new Dictionary<string, string>
        {
            //{ "expires_at", "2026-01-08T12:00:14.6787141Z" }
        };

        // Act
        var result = service.IsRefreshToken(values);

        // Assert
        Console.WriteLine(result);
    }

    [TestMethod]
    public async Task RefreshToken_IsSuccess()
    {
        // Arrange
        var service = new OAuth2TokenService(InvocationContext);
        var values = new Dictionary<string, string>
        {
            { "refresh_token", "609ea89a8caa723639a67b5ae5f0082a5f451717d683aa34d70b4c271b65c16c" },
            { "api_endpoint", "https://kavuntransportation.zendesk.com/" }
        };

        // Act
        var result = service.RefreshToken(values, CancellationToken.None);

        // Assert
        Console.WriteLine(result);
    }
}
