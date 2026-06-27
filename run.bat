@echo off
chcp 65001 > nul
echo.
echo  ████████╗██╗  ██╗███████╗███╗   ███╗███████╗
echo  ╚══██╔══╝██║  ██║██╔════╝████╗ ████║██╔════╝
echo     ██║   ███████║█████╗  ██╔████╔██║█████╗
echo     ██║   ██╔══██║██╔══╝  ██║╚██╔╝██║██╔══╝
echo     ██║   ██║  ██║███████╗██║ ╚═╝ ██║███████╗
echo     ╚═╝   ╚═╝  ╚═╝╚══════╝╚═╝     ╚═╝╚══════╝
echo.
echo  ███████╗ ██████╗ ██████╗  ██████╗ ███████╗
echo  ██╔════╝██╔═══██╗██╔══██╗██╔════╝ ██╔════╝
echo  █████╗  ██║   ██║██████╔╝██║  ███╗█████╗
echo  ██╔══╝  ██║   ██║██╔══██╗██║   ██║██╔══╝
echo  ██║     ╚██████╔╝██║  ██║╚██████╔╝███████╗
echo  ╚═╝      ╚═════╝ ╚═╝  ╚═╝ ╚═════╝ ╚══════╝
echo.
echo  Редактор тем рабочего стола для Windows
echo  ─────────────────────────────────────────────
echo.

:: Проверяем .NET SDK
where dotnet >nul 2>nul
if errorlevel 1 (
    echo  [ОШИБКА] .NET SDK не найден!
    echo.
    echo  Установите .NET 6+ SDK:
    echo  https://dotnet.microsoft.com/download
    echo.
    pause
    exit /b 1
)

echo  [1/3] Проверка зависимостей...
dotnet --version

echo.
echo  [2/3] Сборка проекта...
dotnet build ThemeForge.csproj -c Release --nologo -v quiet

if errorlevel 1 (
    echo.
    echo  [ОШИБКА] Сборка не удалась. Проверьте вывод выше.
    pause
    exit /b 1
)

echo.
echo  [3/3] Запуск ThemeForge...
echo.
dotnet run --project ThemeForge.csproj -c Release --no-build

pause
