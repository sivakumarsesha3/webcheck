using PortfolioVault.Core.DTOs;

namespace PortfolioVault.Core.Abstractions;

public interface IPortfolioContext
{
    Guid ActivePortfolioId { get; }
    void SetActivePortfolio(Guid portfolioId);
}

public interface IEncryptionService
{
    byte[] Protect(byte[] plaintext);
    byte[] Unprotect(byte[] cipherText);
}

public interface IImportParser
{
    Task<ImportReviewResult> ParseIciciAsync(Stream input, CancellationToken ct);
    Task<ImportReviewResult> ParseNsdlAsync(Stream input, CancellationToken ct);
}

public interface IValuationService
{
    Task<DashboardSummary> BuildSummaryAsync(Guid portfolioId, CancellationToken ct);
}

public interface IDesignExportService
{
    Task<string> ExportMarkdownAsync(CancellationToken ct);
}
