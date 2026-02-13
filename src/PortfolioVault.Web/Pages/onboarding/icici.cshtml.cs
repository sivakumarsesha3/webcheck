using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioVault.Core.Abstractions;

namespace PortfolioVault.Web.Pages.Imports
{
    public class IciciModel : PageModel
    {
        private readonly IImportParser _parser;

        public IciciModel(IImportParser parser)
        {
            _parser = parser;
        }

        [BindProperty]
        public IFormFile? File { get; set; }

        public string? Message { get; set; }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync(CancellationToken ct)
        {
            if (File == null || File.Length == 0)
            {
                Message = "No file selected.";
                return Page();
            }

            await using var stream = File.OpenReadStream();
            var result = await _parser.ParseIciciAsync(stream, ct);
            Message = $"Parsed {result?.Transactions?.Count ?? 0} ICICI transactions.";
            // TODO: Save transactions to DB via context
            return Page();
        }
    }
}
