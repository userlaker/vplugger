# ThemeForge — Установка и запуск
# Запускать: правой кнопкой -> "Запустить с помощью PowerShell"

$Host.UI.RawUI.WindowTitle = "ThemeForge Installer"

Write-Host ""
Write-Host "  ╔══════════════════════════════════════╗" -ForegroundColor Cyan
Write-Host "  ║         ThemeForge Installer         ║" -ForegroundColor Cyan
Write-Host "  ║   Редактор тем рабочего стола v1.0  ║" -ForegroundColor Cyan
Write-Host "  ╚══════════════════════════════════════╝" -ForegroundColor Cyan
Write-Host ""

# ── Проверка .NET ───────────────────────────────────────────────
Write-Host "  [*] Проверка .NET SDK..." -ForegroundColor Yellow

$dotnetVersion = $null
try {
    $dotnetVersion = (dotnet --version 2>$null)
} catch {}

if (-not $dotnetVersion) {
    Write-Host "  [!] .NET SDK не установлен!" -ForegroundColor Red
    Write-Host ""
    Write-Host "  Устанавливаем автоматически через winget..." -ForegroundColor Yellow
    
    try {
        winget install Microsoft.DotNet.SDK.8 --accept-source-agreements --accept-package-agreements
        Write-Host "  [+] .NET SDK установлен!" -ForegroundColor Green
        Write-Host "  [!] Перезапустите PowerShell и запустите скрипт снова." -ForegroundColor Yellow
        Read-Host "  Нажмите Enter для выхода"
        exit 0
    } catch {
        Write-Host "  [!] Не удалось установить автоматически." -ForegroundColor Red
        Write-Host "  Скачайте вручную: https://dotnet.microsoft.com/download" -ForegroundColor White
        Start-Process "https://dotnet.microsoft.com/download"
        Read-Host "  Нажмите Enter для выхода"
        exit 1
    }
} else {
    Write-Host "  [+] Найден .NET $dotnetVersion" -ForegroundColor Green
}

# ── Создание папок ───────────────────────────────────────────────
Write-Host ""
Write-Host "  [*] Создание папок..." -ForegroundColor Yellow

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$folders = @("Wallpapers", "Music", "Assets")

foreach ($folder in $folders) {
    $path = Join-Path $scriptDir $folder
    if (-not (Test-Path $path)) {
        New-Item -ItemType Directory -Path $path | Out-Null
        Write-Host "      Создана: $folder\" -ForegroundColor DarkGray
    }
}

# ── Сборка ───────────────────────────────────────────────────────
Write-Host ""
Write-Host "  [*] Сборка проекта..." -ForegroundColor Yellow

Set-Location $scriptDir
$buildResult = dotnet build ThemeForge.csproj -c Release --nologo 2>&1

if ($LASTEXITCODE -ne 0) {
    Write-Host "  [!] Ошибка сборки:" -ForegroundColor Red
    Write-Host $buildResult -ForegroundColor DarkRed
    Read-Host "  Нажмите Enter для выхода"
    exit 1
}

Write-Host "  [+] Сборка успешна!" -ForegroundColor Green

# ── Запуск ───────────────────────────────────────────────────────
Write-Host ""
Write-Host "  [+] Запуск ThemeForge..." -ForegroundColor Green
Write-Host ""
Write-Host "  ┌─────────────────────────────────────┐" -ForegroundColor DarkGray
Write-Host "  │  Для музыки: добавь .mp3 в Music\   │" -ForegroundColor DarkGray
Write-Host "  │  Для обоев:  добавь .png в Wallpapers│" -ForegroundColor DarkGray
Write-Host "  └─────────────────────────────────────┘" -ForegroundColor DarkGray
Write-Host ""

dotnet run --project ThemeForge.csproj -c Release --no-build
