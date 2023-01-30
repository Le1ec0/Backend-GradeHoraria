using Microsoft.Identity.Client;

public class MsalClient
{
    public MsalClient()
    {
        var clientId = "your-client-id";
        var tenantId = "your-tenant-id";
        var clientSecret = "your-client-secret";
        var authority = $"https://login.microsoftonline.com/{tenantId}";
        var resource = "https://graph.microsoft.com";

        var clientCredential = new ClientCredential(clientId, clientSecret);
        var authContext = new AuthenticationContext(authority);
        var result = await authContext.AcquireTokenAsync(resource, clientCredential);
        var accessToken = result.AccessToken;
    }
}