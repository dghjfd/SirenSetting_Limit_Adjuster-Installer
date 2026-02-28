@echo off
chcp 65001 >nul
cd /d "%~dp0"
echo ========================================
echo   TEA Siren Installer - 发布单文件 EXE
echo ========================================
echo.

if not exist "EXE" mkdir EXE
if exist "Logo.png" powershell -NoProfile -ExecutionPolicy Bypass -File "%~dp0PngToIco.ps1"
dotnet publish -c Release -r win-x64 --self-contained true -o "%~dp0EXE" -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=none -p:DebugSymbols=false

if %ERRORLEVEL% EQU 0 (
    if exist "EXE\TEA_siren_v*.exe" (
        echo 发布成功! 输出: EXE\TEA_siren_v*.exe
        explorer "%~dp0EXE"
    ) else (
        echo 未生成 EXE，请确认已安装 .NET 8 SDK: dotnet --version
        pause
    )
) else (
    echo 发布失败! 请查看上方报错，确认已安装 .NET 8 SDK。
    pause
)
