using System.Diagnostics;
using PortfolioVault.Updater.Services;

var currentVersion = "1.0.0";
var updateJsonUrl = Environment.GetEnvironmentVariable("PV_UPDATE_URL") ?? "https://example.com/updates.json";

using var http = new HttpClient();
var service = new UpdateService(http);
var manifest = await service.GetLatestAsync(updateJsonUrl, CancellationToken.None);

if (manifest is not null && manifest.LatestVersion != currentVersion)
{
    var download = await service.DownloadAsync(manifest, AppContext.BaseDirectory, CancellationToken.None);
    Console.WriteLine($"Downloaded update package to {download}");
    // Run installer, replace binaries, then relaunch host app.
}

Process.Start(new ProcessStartInfo
{
    FileName = Path.Combine(AppContext.BaseDirectory, "PortfolioVault.Host.exe"),
    UseShellExecute = true
});
