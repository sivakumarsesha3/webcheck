# PortfolioVault (.NET 8) - Windows Desktop + Localhost Web UI

PortfolioVault is a local-first architecture for a privacy-preserving portfolio and net-worth app.
It uses a Windows EXE host for Microsoft sign-in and launches a browser UI hosted on `https://localhost:<port>`.

## Solution layout

- `src/PortfolioVault.Host` - Windows EXE bootstrapper
  - Requires Microsoft sign-in (MSAL)
  - Starts local web server process
  - Opens default browser
- `src/PortfolioVault.Web` - Razor Pages / Blazor Server app
  - Dashboard, imports, settings pages
  - Local SQLite + migration-on-start
  - Import API endpoints
- `src/PortfolioVault.Core` - Domain entities + contracts
- `src/PortfolioVault.Infrastructure` - EF Core + DPAPI + services
- `src/PortfolioVault.Updater` - updater launcher EXE skeleton
- `scripts/build-windows.ps1` - one-command Windows publish + ZIP bundle

## Security model

- Microsoft login is mandatory before UI startup.
- Web app startup validates host-auth context through `PV_AUTH_OBJECT_ID`.
- Local encryption helper uses DPAPI (`CurrentUser`) for key/blob protection.
- DB file is local only under `%USERPROFILE%\PortfolioVault`.

## Build on your Windows desktop (produces EXE bundle)

1. Install **.NET 8 SDK** on Windows.
2. Open PowerShell at repository root.
3. Run:

```powershell
.\scripts\build-windows.ps1
```

This creates:

- `artifacts/PortfolioVault-Windows/PortfolioVault.Host.exe`
- `artifacts/PortfolioVault-Windows/PortfolioVault.Updater.exe`
- `artifacts/PortfolioVault-Windows/web/*`
- `artifacts/PortfolioVault-Windows.zip`

## Run

Set auth configuration first:

```powershell
$env:PV_MSA_CLIENT_ID = "<your-app-registration-client-id>"
$env:PV_MSA_TENANT_ID = "common"
```

Then launch either:

- `PortfolioVault.Updater.exe` (recommended), or
- `PortfolioVault.Host.exe`

## Included representative features

- Multi-project architecture for host/web/core/infrastructure/updater.
- Domain entities for portfolios, owners, accounts, instruments, properties, transactions, holdings, prices, FX, reminders, backups.
- Import parsing skeletons (ICICI and NSDL placeholder extraction) with review page.
- Dashboard valuation summary service.
- Import reminder background service.
- Perplexity-safe design export page (`/Settings/DesignExport`) with schema/templates/API summary.

## Notes

This repo is a strong implementation starter. To make it production-ready, complete:

- Full parsers (Excel/PDF/OCR template engines + fuzzy matching confirmation UI)
- identity mismatch enforcement (PAN/client IDs) during imports
- encrypted backup/restore `.pbackup`
- price/FX provider jobs
- onboarding wizard and portfolio selector persistence
- comprehensive tests and installer pipeline
