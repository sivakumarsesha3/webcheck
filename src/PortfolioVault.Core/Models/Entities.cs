namespace PortfolioVault.Core.Models;

public sealed class Portfolio
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string BaseCurrency { get; set; } = "USD";
    public bool IsArchived { get; set; }
    public PortfolioOwner Owner { get; set; } = default!;
    public ICollection<Account> Accounts { get; set; } = new List<Account>();
}

public sealed class PortfolioOwner
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PortfolioId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string? Pan { get; set; }
    public string? NsdlClientId { get; set; }
    public string? BrokerClientId { get; set; }
    public string MicrosoftObjectId { get; set; } = string.Empty;
}

public sealed class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PortfolioId { get; set; }
    public string AccountType { get; set; } = "Bank";
    public string ProviderName { get; set; } = string.Empty;
    public string MaskedNumber { get; set; } = string.Empty;
    public string Currency { get; set; } = "USD";
}

public sealed class Instrument
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PortfolioId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Isin { get; set; }
    public string? Ticker { get; set; }
    public string AssetClass { get; set; } = "Equity";
    public string Currency { get; set; } = "USD";
}

public sealed class Property
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PortfolioId { get; set; }
    public string Location { get; set; } = string.Empty;
    public string PropertyType { get; set; } = "Residential";
    public decimal PurchasePrice { get; set; }
    public decimal FairValue { get; set; }
    public string Currency { get; set; } = "USD";
}

public sealed class PortfolioTransaction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PortfolioId { get; set; }
    public Guid? AccountId { get; set; }
    public Guid? InstrumentId { get; set; }
    public DateOnly TransactionDate { get; set; }
    public string TransactionType { get; set; } = "Trade";
    public string Side { get; set; } = "Buy";
    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal Fees { get; set; }
    public string Currency { get; set; } = "USD";
    public string Fingerprint { get; set; } = string.Empty;
}

public sealed class HoldingSnapshot
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PortfolioId { get; set; }
    public Guid AccountId { get; set; }
    public Guid InstrumentId { get; set; }
    public DateOnly SnapshotDate { get; set; }
    public decimal Quantity { get; set; }
}

public sealed class Price
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid InstrumentId { get; set; }
    public DateOnly PriceDate { get; set; }
    public decimal Value { get; set; }
    public string Currency { get; set; } = "USD";
    public string Source { get; set; } = "Manual";
}

public sealed class FxRate
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateOnly Date { get; set; }
    public string FromCurrency { get; set; } = "USD";
    public string ToCurrency { get; set; } = "USD";
    public decimal Rate { get; set; } = 1m;
    public string Source { get; set; } = "Manual";
}

public sealed class PortfolioSettings
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid PortfolioId { get; set; }
    public int ReminderDays { get; set; } = 15;
    public bool RemindForIcici { get; set; } = true;
    public bool RemindForNsdl { get; set; } = true;
    public bool RemindForBank { get; set; } = true;
    public DateTimeOffset? LastIciciImportUtc { get; set; }
    public DateTimeOffset? LastNsdlImportUtc { get; set; }
    public DateTimeOffset? LastBankImportUtc { get; set; }
}

public sealed class BackupMetadata
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTimeOffset CreatedUtc { get; set; }
    public string FilePath { get; set; } = string.Empty;
    public string AppVersion { get; set; } = string.Empty;
}
