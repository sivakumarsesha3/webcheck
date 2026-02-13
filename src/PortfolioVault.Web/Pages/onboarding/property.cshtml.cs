using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioVault.Core.Abstractions;

namespace PortfolioVault.Web.Pages.Imports
{
    public class PropertyModel : PageModel
    {
        private readonly IPortfolioContext _context;

        public PropertyModel(IPortfolioContext context)
        {
            _context = context;
        }

        public string? Message { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(
            DateTime PurchaseDate,
            string Description,
            decimal PurchasePrice,
            CancellationToken ct)
        {
            // TODO: call valuation provider; for now, use PurchasePrice as valuation.
            var valuation = PurchasePrice;

            // TODO: persist as new Property entity via _context
            Message = $"Property added with valuation {valuation:F2}.";
            await Task.CompletedTask;
            return Page();
        }
    }
}
