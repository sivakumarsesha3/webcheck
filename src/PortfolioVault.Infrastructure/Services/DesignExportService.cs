using Microsoft.EntityFrameworkCore;
using PortfolioVault.Core.Abstractions;
using PortfolioVault.Infrastructure.Data;

namespace PortfolioVault.Infrastructure.Services;

public sealed class DesignExportService(PortfolioDbContext db) : IDesignExportService
{
    public async Task<string> ExportMarkdownAsync(CancellationToken ct)
    {
        var entityTypes = db.Model.GetEntityTypes()
            .OrderBy(t => t.GetTableName())
            .Select(t => $"- {t.GetTableName()}: {string.Join(", ", t.GetProperties().Select(p => $"{p.Name}({p.ClrType.Name})"))}");

        var lines = new List<string>
        {
            "# PortfolioVault Design Export",
            $"Generated: {DateTimeOffset.UtcNow:O}",
            "## Tech Stack",
            "- .NET 8",
            "- ASP.NET Core Blazor Server",
            "- EF Core + SQLite",
            "- MSAL.NET desktop auth",
            "- DPAPI for local encryption",
            "## Schema",
        };

        lines.AddRange(entityTypes);

        lines.AddRange([
            "## Import Templates",
            "- ICICI: Trade Date, Symbol, Buy/Sell, Qty, Rate, Brokerage, STT",
            "- NSDL CAS: Investor details + holdings table sections",
            "- Bank statements: Date, Description, Debit, Credit, Balance",
            "## API Surface",
            "- /api/imports/icici: upload and parse ICICI CSV/Excel",
            "- /api/imports/nsdl: upload and parse NSDL text/PDF extracted content",
            "- /settings/design-export: generate schema-only export",
            "## Privacy",
            "No row-level data is included in this export."
        ]);

        await Task.CompletedTask;
        return string.Join(Environment.NewLine, lines);
    }
}
