# update-apk.ps1 -- Build, copy and deploy GarageVelo APK in one step
# Usage: .\update-apk.ps1 [-Config Release|Debug]
param(
    [string]$Config = "Release"
)

$ErrorActionPreference = "Stop"
$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

Write-Host "=== GarageVelo - Update APK ===" -ForegroundColor Cyan
Write-Host ""

# 1. Build
Write-Host "[1/3] Build APK ($Config)..." -ForegroundColor Yellow
& "$ScriptDir\build-apk.ps1" -Config $Config

# 2. Check APK exists
$ApkPath = Join-Path $ScriptDir "..\livrable\GarageVelo.apk"
if (-not (Test-Path $ApkPath)) {
    Write-Host "APK not found in livrable/. Build may have failed." -ForegroundColor Red
    exit 1
}

# 3. Deploy to device
Write-Host ""
Write-Host "[2/3] Deploy to device..." -ForegroundColor Yellow
& "$ScriptDir\deploy-apk.ps1"

Write-Host ""
Write-Host "[3/3] Done!" -ForegroundColor Green
