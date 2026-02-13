using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioVault.Core.Abstractions;

namespace PortfolioVault.Web.Pages.Imports
{
    public class NsdlModel : PageModel
    {
        private readonly IImportParser _parser;

        public NsdlModel(IImportParser parser)
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
            var result = await _parser.ParseNsdlAsync(stream, ct);
            Message = $"Parsed {result?.Holdings?.Count ?? 0} NSDL holdings.";
            // TODO: Save holdings to DB via context
            return Page();
        }
    }
}
