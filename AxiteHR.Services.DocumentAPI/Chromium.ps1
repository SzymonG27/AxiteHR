param(
    [string]$ChromiumVersion = "113.0.5672.63",
    [string]$Platform        = "linux64"  # możliwe: win64, linux64, mac-x64
)

$ErrorActionPreference = "Stop"

# Źródło Chrome for Testing
$BaseUrl   = "https://storage.googleapis.com/chrome-for-testing-public"
$ZipName   = "chrome.zip"
$OutDir    = Join-Path $PSScriptRoot "Chromium"
$DestDir   = Join-Path $OutDir "chrome-linux"
$TempExtractDir = Join-Path $OutDir "tmp-extract"
$ZipPath   = Join-Path $PSScriptRoot $ZipName
$Download  = "$BaseUrl/$ChromiumVersion/$Platform/chrome-$Platform.zip"

$ChromeBinary = Join-Path $DestDir "chrome"

if (-Not (Test-Path $ChromeBinary)) {
    Write-Host "- Downloading Chrome for Testing $ChromiumVersion ($Platform)..."
    curl.exe -L $Download -o $ZipPath

    Write-Host "- Unpacking..."
    if (Test-Path $DestDir) {
        Remove-Item $DestDir -Recurse -Force
    }

    if (Test-Path $TempExtractDir) {
        Remove-Item $TempExtractDir -Recurse -Force
    }

    New-Item -ItemType Directory -Path $TempExtractDir | Out-Null
    Expand-Archive -Path $ZipPath -DestinationPath $TempExtractDir -Force

    Move-Item -Path (Join-Path $TempExtractDir "chrome-linux64\*") -Destination $DestDir -Force

    Remove-Item $TempExtractDir -Recurse -Force
    Remove-Item $ZipPath -Force

    Write-Host "- Chrome downloaded to $DestDir"
} else {
    Write-Host "- Chrome already exists."
}