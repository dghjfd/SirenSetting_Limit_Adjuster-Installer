@echo off
cd /d "%~dp0"
dotnet publish -c Release -r win-x64 --self-contained true -o EXE -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=none -p:DebugSymbols=false
if exist EXE start "" EXE
