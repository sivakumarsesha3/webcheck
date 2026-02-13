using System.Diagnostics;

namespace PortfolioVault.Host.Hosting;

public sealed class WebServerBootstrapper
{
    public Process Start(int port, string microsoftObjectId)
    {
        var webFolder = Path.Combine(AppContext.BaseDirectory, "web");
        var webExe = Path.Combine(webFolder, "PortfolioVault.Web.exe");
        var webDll = Path.Combine(webFolder, "PortfolioVault.Web.dll");

        ProcessStartInfo startInfo;

        if (File.Exists(webExe))
        {
            startInfo = new ProcessStartInfo
            {
                FileName = webExe,
                Arguments = $"--urls https://localhost:{port}",
                WorkingDirectory = webFolder,
                UseShellExecute = false
            };
        }
        else
        {
            startInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"\"{webDll}\" --urls https://localhost:{port}",
                WorkingDirectory = webFolder,
                UseShellExecute = false
            };
        }

        startInfo.Environment["PV_AUTH_OBJECT_ID"] = microsoftObjectId;

        var process = Process.Start(startInfo) ?? throw new InvalidOperationException("Unable to start local web server.");

        Process.Start(new ProcessStartInfo
        {
            FileName = $"https://localhost:{port}/Dashboard",
            UseShellExecute = true
        });

        return process;
    }
}
