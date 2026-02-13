using PortfolioVault.Core.Abstractions;

namespace PortfolioVault.Infrastructure.Services;

public sealed class PortfolioContext : IPortfolioContext
{
    private Guid _activePortfolioId;
    public Guid ActivePortfolioId => _activePortfolioId;
    public void SetActivePortfolio(Guid portfolioId) => _activePortfolioId = portfolioId;
}
