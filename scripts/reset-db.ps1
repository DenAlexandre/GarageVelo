# reset-db.ps1 -- Drop and recreate the GarageVelo database (migrations + seed)
$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ProjectDir = Join-Path $ScriptDir "..\GarageVelo.Api"

Write-Host "=== GarageVelo - Reset Database ===" -ForegroundColor Cyan

# Drop the existing database (--force skips confirmation)
Write-Host "[1/2] Dropping database..."
try {
    dotnet ef database drop --project $ProjectDir --force 2>$null
} catch {
    Write-Host "  (database did not exist, skipping)" -ForegroundColor Yellow
}

# Re-apply all migrations
Write-Host "[2/2] Applying migrations..."
dotnet ef database update --project $ProjectDir

Write-Host ""
Write-Host "Done! Database recreated. Start the server to seed data." -ForegroundColor Green
