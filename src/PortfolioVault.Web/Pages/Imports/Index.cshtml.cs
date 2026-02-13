using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PortfolioVault.Core.Abstractions;
using PortfolioVault.Core.DTOs;

namespace PortfolioVault.Web.Pages.Imports;

public sealed class IndexModel(IImportParser parser) : PageModel
{
    [BindProperty]
    public IFormFile Upload { get; set; } = default!;

    [BindProperty]
    public string Source { get; set; } = "ICICI";

    public ImportReviewResult? Result { get; private set; }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (Upload is null || Upload.Length == 0)
            return Page();

        await using var stream = Upload.OpenReadStream();
        Result = Source == "NSDL"
            ? await parser.ParseNsdlAsync(stream, ct)
            : await parser.ParseIciciAsync(stream, ct);

        return Page();
    }
}
