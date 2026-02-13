namespace PortfolioVault.Core.DTOs;

public sealed record ParsedImportRow(
    DateOnly Date,
    string Instrument,
    string Side,
    decimal Quantity,
    decimal Price,
    decimal Fees,
    string Currency,
    string SourceLine);

public sealed record ImportReviewResult(
    IReadOnlyList<ParsedImportRow> ParsedRows,
    IReadOnlyList<string> UnparsedLines,
    string? IdentityName,
    string? IdentityPan);

public sealed record DashboardSummary(
    decimal CashBase,
    decimal SecuritiesBase,
    decimal PropertyBase,
    decimal NetWorthBase,
    IReadOnlyDictionary<string, decimal> Allocation);
