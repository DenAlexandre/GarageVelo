# deploy-apk.ps1 -- Install GarageVelo APK on connected Android device
# Usage: .\deploy-apk.ps1
$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$ApkPath = Join-Path $ScriptDir "..\livrable\GarageVelo.apk"

Write-Host "=== GarageVelo - Deploy APK ===" -ForegroundColor Cyan

# Check APK exists
if (-not (Test-Path $ApkPath)) {
    Write-Host "APK not found: $ApkPath" -ForegroundColor Red
    Write-Host "Run build-apk.ps1 first."
    exit 1
}

$size = "{0:N1} MB" -f ((Get-Item $ApkPath).Length / 1MB)
Write-Host "  APK: $ApkPath ($size)"
Write-Host ""

# Find adb: PATH first, then standard Android SDK locations
$adb = (Get-Command adb -ErrorAction SilentlyContinue).Source
if (-not $adb) {
    $candidates = @(
        "$env:LOCALAPPDATA\Android\Sdk\platform-tools\adb.exe",
        "${env:ProgramFiles(x86)}\Android\android-sdk\platform-tools\adb.exe",
        "$env:ProgramFiles\Android\android-sdk\platform-tools\adb.exe",
        "$env:ANDROID_HOME\platform-tools\adb.exe"
    )
    foreach ($c in $candidates) {
        if (Test-Path $c) { $adb = $c; break }
    }
}

if (-not $adb) {
    Write-Host "adb not found. Install Android SDK platform-tools or set ANDROID_HOME." -ForegroundColor Red
    exit 1
}

Write-Host "  adb: $adb"
Write-Host ""

# Check device is connected
$devices = & $adb devices | Select-Object -Skip 1 | Where-Object { $_ -match "\S" }
if (-not $devices) {
    Write-Host "No Android device detected." -ForegroundColor Red
    Write-Host "Connect a device via USB (with USB debugging enabled) or start an emulator."
    exit 1
}

Write-Host "Connected devices:"
$devices | ForEach-Object { Write-Host "  $_" }
Write-Host ""

# Push APK to device Downloads folder
$destPath = "/sdcard/Download/GarageVelo.apk"
Write-Host "Copying APK to device ($destPath)..."
& $adb push $ApkPath $destPath

if ($LASTEXITCODE -eq 0) {
    Write-Host ""
    Write-Host "APK copied to device!" -ForegroundColor Green
    Write-Host "Open the file manager on the telephone and install from Telechargements/GarageVelo.apk"
} else {
    Write-Host ""
    Write-Host "Copy failed (exit code $LASTEXITCODE)." -ForegroundColor Red
}
