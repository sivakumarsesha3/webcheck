using Microsoft.EntityFrameworkCore;
using PortfolioVault.Core.Abstractions;
using PortfolioVault.Infrastructure.Data;
using PortfolioVault.Infrastructure.Services;
using PortfolioVault.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Ensure desktop auth has passed a signed-in object ID
var signedInObjectId = Environment.GetEnvironmentVariable("PV_AUTH_OBJECT_ID");
if (string.IsNullOrWhiteSpace(signedInObjectId))
    throw new InvalidOperationException("Desktop Microsoft authentication is required before starting the local UI.");

// Base folder for local data
var baseFolder = Path.Combine(
    Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
    "PortfolioVault");
Directory.CreateDirectory(baseFolder);

// UI
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddHttpClient();

// Core services
builder.Services.AddSingleton<IPortfolioContext, PortfolioContext>();
builder.Services.AddScoped<IImportParser, ImportParser>();
builder.Services.AddScoped<IValuationService, ValuationService>();
builder.Services.AddScoped<IDesignExportService, DesignExportService>();
// Temporarily disable the reminder background service to avoid Settings table query.
// builder.Services.AddHostedService<ImportReminderService>();

// Database
builder.Services.AddDbContext<PortfolioDbContext>(opt =>
{
    var dbPath = Path.Combine(baseFolder, "portfoliovault.db");
    opt.UseSqlite($"Data Source={dbPath}");
});

var app = builder.Build();

// Apply migrations on startup
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
    await db.Database.MigrateAsync();
}

// Pipeline
app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapRazorPages();
app.MapBlazorHub();

// Routes
app.MapGet("/", () => Results.Redirect("/Dashboard"));
app.MapGet("/api/session", () => Results.Ok(new { objectId = signedInObjectId }));

app.MapPost("/api/imports/icici", async (IFormFile file, IImportParser parser, CancellationToken ct) =>
{
    await using var stream = file.OpenReadStream();
    var result = await parser.ParseIciciAsync(stream, ct);
    return Results.Ok(result);
});

app.MapPost("/api/imports/nsdl", async (IFormFile file, IImportParser parser, CancellationToken ct) =>
{
    await using var stream = file.OpenReadStream();
    var result = await parser.ParseNsdlAsync(stream, ct);
    return Results.Ok(result);
});

await app.RunAsync();

namespace PortfolioVault.Web
{
    public partial class Program { }
}
