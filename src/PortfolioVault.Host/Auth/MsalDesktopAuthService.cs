using Microsoft.Identity.Client;

namespace PortfolioVault.Host.Auth;

public sealed class MsalDesktopAuthService
{
    private readonly IPublicClientApplication _app;
    private readonly string[] _scopes = ["User.Read"];

    public MsalDesktopAuthService(string clientId, string tenantId)
    {
        _app = PublicClientApplicationBuilder
            .Create(clientId)
            .WithAuthority(AzureCloudInstance.AzurePublic, tenantId)
            .WithDefaultRedirectUri()
            .Build();
    }

    public async Task<string?> AuthenticateAsync(CancellationToken ct)
    {
        try
        {
            var account = (await _app.GetAccountsAsync()).FirstOrDefault();
            var silent = await _app.AcquireTokenSilent(_scopes, account).ExecuteAsync(ct);
            return silent.Account.HomeAccountId.ObjectId;
        }
        catch (MsalUiRequiredException)
        {
            var interactive = await _app.AcquireTokenInteractive(_scopes).ExecuteAsync(ct);
            return interactive.Account.HomeAccountId.ObjectId;
        }
    }
}
