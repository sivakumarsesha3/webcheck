using Microsoft.EntityFrameworkCore;
using PortfolioVault.Core.Abstractions;
using PortfolioVault.Core.DTOs;
using PortfolioVault.Infrastructure.Data;

namespace PortfolioVault.Infrastructure.Services;

public sealed class ValuationService(PortfolioDbContext db) : IValuationService
{
    public async Task<DashboardSummary> BuildSummaryAsync(Guid portfolioId, CancellationToken ct)
    {
        var cash = await db.Transactions.Where(x => x.PortfolioId == portfolioId && x.TransactionType == "Cash")
            .SumAsync(x => (x.Price * x.Quantity), ct);

        var securities = await db.Transactions.Where(x => x.PortfolioId == portfolioId && x.TransactionType == "Trade")
            .SumAsync(x => (x.Price * x.Quantity), ct);

        var property = await db.Properties.Where(x => x.PortfolioId == portfolioId).SumAsync(x => x.FairValue, ct);

        var netWorth = cash + securities + property;
        var allocation = new Dictionary<string, decimal>
        {
            ["Cash"] = netWorth == 0 ? 0 : cash / netWorth,
            ["Securities"] = netWorth == 0 ? 0 : securities / netWorth,
            ["Property"] = netWorth == 0 ? 0 : property / netWorth
        };

        return new DashboardSummary(cash, securities, property, netWorth, allocation);
    }
}
