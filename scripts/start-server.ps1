# start-server.ps1 -- Start the GarageVelo API server in the background
$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$RootDir = Split-Path -Parent $ScriptDir
$ProjectDir = Join-Path $RootDir "GarageVelo.Api"
$PidFile = Join-Path $RootDir ".server.pid"
$LogFile = Join-Path $RootDir ".server.log"

# Check if already running
if (Test-Path $PidFile) {
    $OldPid = Get-Content $PidFile
    try {
        $proc = Get-Process -Id $OldPid -ErrorAction Stop
        Write-Host "Server already running (PID $OldPid)." -ForegroundColor Yellow
        Write-Host "Use stop-server.ps1 to stop it first."
        exit 1
    } catch {
        Remove-Item $PidFile -Force
    }
}

Write-Host "=== GarageVelo -- Starting API Server ===" -ForegroundColor Cyan
Write-Host "  Project: $ProjectDir"
Write-Host "  URLs:    http://localhost:5000 | https://localhost:5001"
Write-Host "  Swagger: http://localhost:5000/swagger"
Write-Host ""

# Start in background, log to file
$process = Start-Process -FilePath "dotnet" `
    -ArgumentList "run", "--project", $ProjectDir, "--urls", "http://localhost:5000;https://localhost:5001" `
    -RedirectStandardOutput $LogFile `
    -RedirectStandardError (Join-Path $RootDir ".server.err.log") `
    -PassThru `
    -WindowStyle Hidden

$process.Id | Out-File -FilePath $PidFile -NoNewline

Write-Host "Server started (PID $($process.Id))" -ForegroundColor Green
Write-Host "Logs: $LogFile"
Write-Host ""
Write-Host "Waiting for server to be ready..."

# Wait up to 15 seconds for the server to respond
for ($i = 1; $i -le 15; $i++) {
    try {
        $null = Invoke-WebRequest -Uri "http://localhost:5000/swagger/index.html" -UseBasicParsing -TimeoutSec 2 -ErrorAction Stop
        Write-Host "Server is ready!" -ForegroundColor Green
        exit 0
    } catch {
        Start-Sleep -Seconds 1
    }
}

Write-Host "Warning: server may not be fully started yet. Check logs: $LogFile" -ForegroundColor Yellow
