# build-apk.ps1 -- Build the GarageVelo Android APK
# Usage: .\build-apk.ps1 [-Config Release|Debug]
param(
    [string]$Config = "Release"
)

$ErrorActionPreference = "Stop"

$ScriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$Project = Join-Path $ScriptDir "..\GarageVelo\GarageVelo.csproj"

Write-Host "=== GarageVelo - Build Android APK ===" -ForegroundColor Cyan
Write-Host "  Configuration: $Config"
Write-Host ""

dotnet publish $Project -f net10.0-android -c $Config

Write-Host ""
Write-Host "=== Build complete ===" -ForegroundColor Green
Write-Host ""

# Find and display the generated APK(s)
$OutputDir = Join-Path $ScriptDir "..\GarageVelo\bin\$Config\net10.0-android\publish"
if (-not (Test-Path $OutputDir)) {
    $OutputDir = Join-Path $ScriptDir "..\GarageVelo\bin"
}

$files = Get-ChildItem -Path $OutputDir -Recurse -Include "*.apk", "*.aab" -ErrorAction SilentlyContinue
if ($files) {
    Write-Host "Output files:"
    foreach ($f in $files) {
        $size = "{0:N1} MB" -f ($f.Length / 1MB)
        Write-Host "  $size  $($f.FullName)"
    }
    # Copy signed APK to livrable/
    $LivrableDir = Join-Path $ScriptDir "..\livrable"
    if (-not (Test-Path $LivrableDir)) {
        New-Item -ItemType Directory -Path $LivrableDir | Out-Null
    }

    $signedApk = $files | Where-Object { $_.Name -like "*-Signed.apk" } | Select-Object -First 1
    if (-not $signedApk) {
        $signedApk = $files | Where-Object { $_.Name -like "*.apk" } | Select-Object -First 1
    }

    if ($signedApk) {
        $dest = Join-Path $LivrableDir "GarageVelo.apk"
        Copy-Item $signedApk.FullName -Destination $dest -Force
        $size = "{0:N1} MB" -f ((Get-Item $dest).Length / 1MB)
        Write-Host ""
        Write-Host "APK copied to livrable:" -ForegroundColor Green
        Write-Host "  $size  $dest"
    }
} else {
    Write-Host "No APK/AAB found in output directory." -ForegroundColor Yellow
}
