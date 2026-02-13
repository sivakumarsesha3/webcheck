using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace PortfolioVault.Updater.Services;

public sealed class UpdateService(HttpClient httpClient)
{
    public async Task<UpdateManifest?> GetLatestAsync(string endpoint, CancellationToken ct)
        => await httpClient.GetFromJsonAsync<UpdateManifest>(endpoint, ct);

    public async Task<string> DownloadAsync(UpdateManifest manifest, string destinationFolder, CancellationToken ct)
    {
        var filePath = Path.Combine(destinationFolder, Path.GetFileName(new Uri(manifest.DownloadUrl).LocalPath));
        await using var response = await httpClient.GetStreamAsync(manifest.DownloadUrl, ct);
        await using var file = File.Create(filePath);
        await response.CopyToAsync(file, ct);
        return filePath;
    }
}

public sealed record UpdateManifest(
    [property: JsonPropertyName("latestVersion")] string LatestVersion,
    [property: JsonPropertyName("downloadUrl")] string DownloadUrl,
    [property: JsonPropertyName("appId")] string AppId);
