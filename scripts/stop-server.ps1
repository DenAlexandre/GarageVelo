# stop-server.ps1 -- Stop the GarageVelo API server
$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Split-Path -Parent $ScriptDir
$PidFile = Join-Path $RootDir ".server.pid"

Write-Host "=== GarageVelo -- Stopping API Server ===" -ForegroundColor Cyan

if (-not (Test-Path $PidFile)) {
    Write-Host "No PID file found. Server is not running (or was started manually)." -ForegroundColor Yellow
    exit 0
}

$ServerPid = [int](Get-Content $PidFile)

try {
    $proc = Get-Process -Id $ServerPid -ErrorAction Stop

    Write-Host "Stopping server (PID $ServerPid)..."

    # Graceful stop
    $proc.Kill()
    $proc.WaitForExit(5000)

    if (-not $proc.HasExited) {
        Write-Host "Force killing..."
        Stop-Process -Id $ServerPid -Force -ErrorAction SilentlyContinue
    }

    Write-Host "Server stopped." -ForegroundColor Green
} catch {
    Write-Host "Server (PID $ServerPid) is not running." -ForegroundColor Yellow
}

Remove-Item $PidFile -Force -ErrorAction SilentlyContinue
