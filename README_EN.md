# TEA Siren Installer

> One-click installer for FiveM SirenSetting Limit Adjuster · Portable single exe · Free, no bundling  
> **中文：** [README.md](README.md)

A graphical installer for the [SirenSetting_Limit_Adjuster](https://github.com/KevinL852/SirenSetting_Limit_Adjuster) plugin. It downloads the ASI from the official GitHub repo and installs it into your FiveM `plugins` folder.

---

## Features

- **Auto scan**: Finds your FiveM `plugins` directory automatically
- **One-click install**: Downloads the latest ASI from GitHub (with proxy fallback if direct connection fails)
- **Install from local file**: If you cannot access GitHub, get the `.asi` file by other means (proxy, mirror, or from someone else), then use **「从本地安装」** (Install from local) to select the file
- **Backup old version**: Backs up existing plugins to `SirenSetting_Limit_Adjuster_备份` before installing
- **Version check**: On startup, checks for a newer release; from v3.6.0 onward, you must update when a new version is available
- **Download progress**: Shows real-time download progress and log during install
- **Single-file portable**: One exe, no separate .NET runtime required (self-contained)

---

## Download

- **Releases**: Download the latest `TEA_siren_v*.exe` from [Releases](https://github.com/dghjfd/SirenSetting_Limit_Adjuster-Installer/releases)
- Run the exe directly; no installation needed.

---

## How to use

1. Run `TEA_siren_v*.exe`; the first time you’ll see a short usage notice.
2. **Choose plugins folder**:
   - Click **「自动扫描」** (Auto scan) to find FiveM’s plugins directory, or
   - Click **「选择目录」** (Choose folder) and select the `plugins` folder (or the `FiveM.app` folder).
3. **Install the plugin**:
   - Click **「安装插件」** (Install plugin) to download from GitHub (or a proxy) and install, or
   - If you can’t access GitHub: get `SirenSetting_Limit_Adjuster_b3751.asi` via proxy/mirror/friend, then click **「从本地安装」** (Install from local) and select that file.
4. Restart or start FiveM after installation for the plugin to take effect.

---

## Build from source

### Requirements

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- Windows (WinForms app, Windows only)

### Steps

1. Clone this repo and go to the project folder:
   ```bash
   cd TEA_siren
   ```
2. Publish as a single-file exe:
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true -o EXE -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=none -p:DebugSymbols=false
   ```
   Or double-click `publish.bat` in the project directory.
3. The built exe will be at `EXE\TEA_siren_v3.6.1.exe` (version follows `Version` in the .csproj).

---

## Security

- This software is **free**. If someone charges you for it, do not pay.
- **What it does**: Only selects a folder, downloads the ASI file, and copies it to the chosen `plugins` directory. It does not modify memory, inject into processes, scan your disk, or bundle anything. No backdoors.
- The process exits when you close the app; it does not run in the background.
- Source code is public; you can review it. If you see odd behavior, it may be a modified repack—get the exe from the official releases.

---

## Thanks & License

- The plugin itself is by [KevinL852/SirenSetting_Limit_Adjuster](https://github.com/KevinL852/SirenSetting_Limit_Adjuster); this repo is only the installer.
- **Author**: TEARLESSVVOID · © Copyright TEARLESSVVOID
- This repository is licensed under [GPL-3.0](LICENSE).
