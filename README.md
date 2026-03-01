# TEA Siren Installer

> One-click installer for FiveM SirenSetting Limit Adjuster · Portable single exe · Free, no bundling  
> **中文：** [README_ZH.md](README_ZH.md)

A graphical installer for the [SirenSetting_Limit_Adjuster](https://github.com/KevinL852/SirenSetting_Limit_Adjuster) plugin. It downloads the ASI from the official GitHub repo and installs it into your FiveM `plugins` folder. **Default UI language is English**; you can switch to 中文 in **Options** tab.

---

## Features

- **Tabs**: Install · Options · Sponsors · Changelog
- **Language**: English (default) and 中文; choice saved in Options
- **Auto scan**: Finds your FiveM `plugins` directory automatically
- **One-click install**: Downloads the latest ASI from GitHub (with proxy fallback if direct connection fails)
- **Install from local file**: If you cannot access GitHub, get the `.asi` file by other means, then use **Install from local** to select the file
- **Backup old version**: Backs up existing plugins before installing
- **Update check**: Optional on startup; in-app **Check update**; if you skip more than 2 versions, update is required
- **In-app update**: Download new version to temp and replace current exe (CDN or direct)
- **Sponsors**: Tab to thank supporters
- **Changelog**: Tab to load and view release history from GitHub
- **Single-file portable**: One exe, no separate .NET runtime (self-contained)

---

## Download

- **Releases**: [Releases](https://github.com/dghjfd/SirenSetting_Limit_Adjuster-Installer/releases) — download the latest `TEA_siren_v*.exe`
- Run the exe directly; no installation needed.

---

## How to use

1. Run `TEA_siren_v*.exe`. First run shows a short usage notice (language follows Options).
2. **Install** tab: Choose plugins folder (Auto scan or Browse), then **Install plugin** or **Install from local**.
3. **Options** tab: Set **Language** (English / 中文) and **Check for update on startup**.
4. **Sponsors** tab: View thanks and sponsor list.
5. **Changelog** tab: Click **Load from GitHub** to fetch release history.
6. Restart or start FiveM after installing the ASI.

---

## Build from source

- **Requirements**: [.NET 8 SDK](https://dotnet.microsoft.com/download), Windows
- **Steps**:
  ```bash
  cd TEA_siren
  dotnet publish -c Release -r win-x64 --self-contained true -o EXE -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=none -p:DebugSymbols=false
  ```
  Or run `publish.bat`. Output: `EXE\TEA_siren_v*.exe`.

---

## Security

- This software is **free**. Do not pay anyone for it.
- It only: selects a folder, downloads the ASI, and copies it to `plugins`. No backdoors, no bundling, no disk scan, no memory/process injection. Exits when closed.
- Source is public; review it. Prefer official releases to avoid modified repacks.

---

## Thanks & License

- Plugin: [KevinL852/SirenSetting_Limit_Adjuster](https://github.com/KevinL852/SirenSetting_Limit_Adjuster). This repo is the installer only.
- **Author**: TEARLESSVVOID · © Copyright TEARLESSVVOID
- License: [GPL-3.0](LICENSE).
