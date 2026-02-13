using Microsoft.EntityFrameworkCore;
using PortfolioVault.Infrastructure.Data;

namespace PortfolioVault.Web.Services;

public sealed class ImportReminderService(IServiceScopeFactory scopeFactory, ILogger<ImportReminderService> logger) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<PortfolioDbContext>();
            var allSettings = await db.Settings.ToListAsync(stoppingToken);
            var now = DateTimeOffset.UtcNow;

            foreach (var item in allSettings)
            {
                var limit = TimeSpan.FromDays(item.ReminderDays);
                if (item.RemindForIcici && now - (item.LastIciciImportUtc ?? DateTimeOffset.MinValue) > limit)
                    logger.LogInformation("ICICI import reminder for portfolio {PortfolioId}", item.PortfolioId);
            }

            await Task.Delay(TimeSpan.FromHours(6), stoppingToken);
        }
    }
}
