using PortfolioVault.Host.Auth;
using PortfolioVault.Host.Hosting;

var clientId = Environment.GetEnvironmentVariable("PV_MSA_CLIENT_ID") ?? "YOUR_CLIENT_ID";
var tenantId = Environment.GetEnvironmentVariable("PV_MSA_TENANT_ID") ?? "common";

var auth = new MsalDesktopAuthService(clientId, tenantId);
var objectId = await auth.AuthenticateAsync(CancellationToken.None);
if (string.IsNullOrWhiteSpace(objectId))
{
    Console.Error.WriteLine("Microsoft sign-in failed; application will exit.");
    return;
}

var port = 5078;
var bootstrapper = new WebServerBootstrapper();
using var webProcess = bootstrapper.Start(port, objectId);

Console.WriteLine("PortfolioVault running. Press Ctrl+C to exit.");
var tcs = new TaskCompletionSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    try
    {
        if (!webProcess.HasExited)
            webProcess.Kill(entireProcessTree: true);
    }
    catch
    {
        // Ignore shutdown errors.
    }

    tcs.TrySetResult();
};

await Task.WhenAny(webProcess.WaitForExitAsync(), tcs.Task);
