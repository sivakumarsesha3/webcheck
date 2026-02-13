param(
  [string]$Configuration = "Release",
  [string]$Runtime = "win-x64",
  [string]$OutputRoot = "artifacts"
)

$ErrorActionPreference = "Stop"

$root = Resolve-Path (Join-Path $PSScriptRoot "..")
Set-Location $root

$publishRoot = Join-Path $root $OutputRoot
$webOut = Join-Path $publishRoot "web"
$hostOut = Join-Path $publishRoot "host"
$updaterOut = Join-Path $publishRoot "updater"
$bundleOut = Join-Path $publishRoot "PortfolioVault-Windows"

if (Test-Path $publishRoot) { Remove-Item $publishRoot -Recurse -Force }
New-Item -ItemType Directory -Path $webOut,$hostOut,$updaterOut,$bundleOut | Out-Null

Write-Host "Publishing web app..."
dotnet publish src/PortfolioVault.Web/PortfolioVault.Web.csproj -c $Configuration -r $Runtime --self-contained true /p:PublishSingleFile=true -o $webOut

Write-Host "Publishing host app..."
dotnet publish src/PortfolioVault.Host/PortfolioVault.Host.csproj -c $Configuration -r $Runtime --self-contained true /p:PublishSingleFile=true -o $hostOut

Write-Host "Publishing updater app..."
dotnet publish src/PortfolioVault.Updater/PortfolioVault.Updater.csproj -c $Configuration -r $Runtime --self-contained true /p:PublishSingleFile=true -o $updaterOut

Copy-Item $webOut (Join-Path $bundleOut "web") -Recurse
Copy-Item $hostOut\PortfolioVault.Host.exe $bundleOut
Copy-Item $updaterOut\PortfolioVault.Updater.exe $bundleOut

@"
Run order:
1) PortfolioVault.Updater.exe (recommended launcher), or
2) PortfolioVault.Host.exe

Prerequisites:
- Microsoft app registration configured via environment variables:
  - PV_MSA_CLIENT_ID
  - PV_MSA_TENANT_ID (or common)
"@ | Set-Content (Join-Path $bundleOut "RUN_FIRST.txt")

Compress-Archive -Path (Join-Path $bundleOut "*") -DestinationPath (Join-Path $publishRoot "PortfolioVault-Windows.zip")

Write-Host "Created bundle: $publishRoot/PortfolioVault-Windows.zip"
