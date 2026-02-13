using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioVault.Core.Abstractions;

namespace PortfolioVault.Web.Pages.Settings;

public sealed class DesignExportModel(IDesignExportService designExportService) : PageModel
{
    public string Markdown { get; private set; } = string.Empty;

    public async Task OnPostAsync(CancellationToken ct)
    {
        Markdown = await designExportService.ExportMarkdownAsync(ct);
    }
}
