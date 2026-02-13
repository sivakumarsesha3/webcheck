using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioVault.Core.Abstractions;
using PortfolioVault.Core.DTOs;

namespace PortfolioVault.Web.Pages.Dashboard;

public sealed class IndexModel(IValuationService valuationService, IPortfolioContext portfolioContext) : PageModel
{
    public DashboardSummary Summary { get; private set; } = new(0, 0, 0, 0, new Dictionary<string, decimal>());
    public string BaseCurrency { get; private set; } = "USD";

    public async Task OnGetAsync(CancellationToken ct)
    {
        if (portfolioContext.ActivePortfolioId == Guid.Empty)
            return;

        Summary = await valuationService.BuildSummaryAsync(portfolioContext.ActivePortfolioId, ct);
    }
}
