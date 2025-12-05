using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Zendesk.Connections;

public class ConnectionValidator(InvocationContext invocationContext) : BaseInvocable(invocationContext), IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
    {
        var client = new ZendeskClient(new() { AuthenticationCredentialsProviders = authProviders });
        var request = new ZendeskRequest("/api/v2/users/me", Method.Get);

        try
        {
            await client.ExecuteWithHandling(request);
            return new()
            {
                IsValid = true
            };
        }
        catch (Exception ex)
        {
            InvocationContext.Logger?.LogError($"[ZendeskConnectionValidator] Connection validation failed ({ex.GetType()}) with the message: {ex.Message}", []);
            return new()
            {
                IsValid = false,
                Message = ex.Message
            };
        }
    }
}