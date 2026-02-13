using System.Globalization;
using PortfolioVault.Core.Abstractions;
using PortfolioVault.Core.DTOs;

namespace PortfolioVault.Infrastructure.Services;

public sealed class ImportParser : IImportParser
{
    public async Task<ImportReviewResult> ParseIciciAsync(Stream input, CancellationToken ct)
    {
        using var reader = new StreamReader(input);
        var lines = new List<string>();
        while (!reader.EndOfStream)
        {
            var line = await reader.ReadLineAsync(ct);
            if (!string.IsNullOrWhiteSpace(line)) lines.Add(line);
        }

        var parsed = new List<ParsedImportRow>();
        var unparsed = new List<string>();

        foreach (var line in lines.Skip(1))
        {
            var cols = line.Split(',');
            if (cols.Length < 7 || !DateOnly.TryParse(cols[0], out var date))
            {
                unparsed.Add(line);
                continue;
            }

            parsed.Add(new ParsedImportRow(
                date,
                cols[1],
                cols[2],
                decimal.Parse(cols[3], CultureInfo.InvariantCulture),
                decimal.Parse(cols[4], CultureInfo.InvariantCulture),
                decimal.Parse(cols[5], CultureInfo.InvariantCulture),
                cols[6],
                line));
        }

        return new ImportReviewResult(parsed, unparsed, null, null);
    }

    public async Task<ImportReviewResult> ParseNsdlAsync(Stream input, CancellationToken ct)
    {
        using var reader = new StreamReader(input);
        var text = await reader.ReadToEndAsync(ct);
        var name = text.Split('\n').FirstOrDefault(l => l.Contains("Name", StringComparison.OrdinalIgnoreCase));
        var pan = text.Split('\n').FirstOrDefault(l => l.Contains("PAN", StringComparison.OrdinalIgnoreCase));

        return new ImportReviewResult(Array.Empty<ParsedImportRow>(), Array.Empty<string>(), name, pan);
    }
}
