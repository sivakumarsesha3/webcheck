using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioVault.Core.Abstractions;

namespace PortfolioVault.Web.Pages.Onboarding
{
    public class IndexModel : PageModel
    {
        private readonly IImportParser _importParser;
        private readonly IPortfolioContext _context;
        private readonly IHttpClientFactory _httpClientFactory;

        public IndexModel(
            IImportParser importParser,
            IPortfolioContext context,
            IHttpClientFactory httpClientFactory)
        {
            _importParser = importParser;
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [TempData] public string? IciciResult { get; set; }
        [TempData] public string? NsdlResult { get; set; }
        [TempData] public string? BankResult { get; set; }
        [TempData] public string? PropertyResult { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostIciciAsync(IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
            {
                IciciResult = "No file selected.";
                return RedirectToPage();
            }

            await using var stream = file.OpenReadStream();
            var result = await _importParser.ParseIciciAsync(stream, ct);
            // TODO: persist result into _context
            IciciResult = $"ICICI file parsed. {result?.Transactions?.Count ?? 0} transactions detected.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostNsdlAsync(IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
            {
                NsdlResult = "No file selected.";
                return RedirectToPage();
            }

            await using var stream = file.OpenReadStream();
            var result = await _importParser.ParseNsdlAsync(stream, ct);
            // TODO: persist result into _context
            NsdlResult = $"NSDL CAS parsed. {result?.Holdings?.Count ?? 0} holdings detected.";
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostBankAsync(IFormFile file, CancellationToken ct)
        {
            if (file == null || file.Length == 0)
            {
                BankResult = "No file selected.";
                return RedirectToPage();
            }

            // TODO: implement bank PDF parsing service
            // For now, just acknowledge upload.
            BankResult = "Bank statement uploaded (parsing to be implemented).";
            await Task.CompletedTask;
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostPropertyAsync(
            DateTime PurchaseDate,
            string Description,
            decimal PurchasePrice,
            CancellationToken ct)
        {
            // TODO: Call external valuation API using Description (city/area)
            var client = _httpClientFactory.CreateClient();
            // Placeholder: try GET to some valuation endpoint; if fails, use PurchasePrice
            decimal valuation = PurchasePrice;

            // TODO: save property into _context.Properties
            PropertyResult = $"Property added. Valuation set to {valuation:F2}.";
            await Task.CompletedTask;
            return RedirectToPage();
        }
    }
}
