using Microsoft.EntityFrameworkCore;
using PortfolioVault.Core.Models;

namespace PortfolioVault.Infrastructure.Data;

public sealed class PortfolioDbContext(DbContextOptions<PortfolioDbContext> options) : DbContext(options)
{
    public DbSet<Portfolio> Portfolios => Set<Portfolio>();
    public DbSet<PortfolioOwner> PortfolioOwners => Set<PortfolioOwner>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Instrument> Instruments => Set<Instrument>();
    public DbSet<Property> Properties => Set<Property>();
    public DbSet<PortfolioTransaction> Transactions => Set<PortfolioTransaction>();
    public DbSet<HoldingSnapshot> HoldingSnapshots => Set<HoldingSnapshot>();
    public DbSet<Price> Prices => Set<Price>();
    public DbSet<FxRate> FxRates => Set<FxRate>();
    public DbSet<PortfolioSettings> Settings => Set<PortfolioSettings>();
    public DbSet<BackupMetadata> Backups => Set<BackupMetadata>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Portfolio>().HasOne(p => p.Owner).WithOne().HasForeignKey<PortfolioOwner>(x => x.PortfolioId);
        modelBuilder.Entity<PortfolioTransaction>().HasIndex(x => new { x.PortfolioId, x.Fingerprint }).IsUnique();
        modelBuilder.Entity<PortfolioOwner>().HasIndex(x => x.MicrosoftObjectId);
    }
}
