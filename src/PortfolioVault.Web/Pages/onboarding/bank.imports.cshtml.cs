using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace PortfolioVault.Web.Pages.Imports
{
    public class BankModel : PageModel
    {
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

            // TODO: implement bank statement parsing/ledger logic
            Message = "Bank statement uploaded (parsing to be implemented).";
            await Task.CompletedTask;
            return Page();
        }
    }
}
