# TEA Siren Installer

> FiveM 警灯上限插件（SirenSetting Limit Adjuster）一键安装工具 · 便携单文件 · 免费无捆绑

基于 [SirenSetting_Limit_Adjuster](https://github.com/KevinL852/SirenSetting_Limit_Adjuster) 官方仓库制作的图形化安装器，方便新手一键将 ASI 插件安装到 FiveM 的 `plugins` 目录。

---

## 功能特点

- **自动扫描**：自动查找本机 FiveM 安装目录下的 `plugins` 文件夹  
- **一键安装**：从 GitHub 下载最新 ASI 并安装，支持国内代理备用（直连失败时自动尝试镜像）  
- **从本地安装**：无法访问 GitHub 时，可先通过其他途径获取 `.asi` 文件，再选择「从本地安装」完成安装  
- **旧版备份**：安装前自动将旧版本备份到 `SirenSetting_Limit_Adjuster_备份` 文件夹  
- **版本检查**：启动时检查是否有新版本；3.6.0 及以上版本检测到更新时需更新后才能继续使用  
- **下载进度**：安装过程中实时显示下载进度与日志  
- **单文件便携**：发布为单个 exe，无需安装 .NET 运行时（已内嵌）

---

## 下载

- **Releases**：在 [Releases](https://github.com/dghjfd/SirenSetting_Limit_Adjuster-Installer/releases) 页面下载最新版 `TEA_siren_v*.exe`  
- 解压或下载后直接运行，无需安装。

---

## 使用方法

1. 运行 `TEA_siren_v*.exe`，首次会弹出使用说明。  
2. **选择 plugins 目录**：  
   - 点击「自动扫描」由程序自动查找 FiveM 的 plugins 目录，或  
   - 点击「选择目录」手动指定 `plugins` 所在文件夹（或选择 `FiveM.app` 目录亦可）。  
3. **安装插件**：  
   - 点击「安装插件」：从 GitHub（或备用代理）下载并安装；  
   - 若无法访问 GitHub：先通过代理、镜像或他人获取 `SirenSetting_Limit_Adjuster_b3751.asi`，再点击「从本地安装」选择该文件。  
4. 安装完成后，按提示重启或启动 FiveM 即可生效。

---

## 从源码编译

### 环境要求

- [.NET 8 SDK](https://dotnet.microsoft.com/download)  
- Windows（本程序为 WinForms，仅支持 Windows）

### 编译步骤

1. 克隆本仓库后进入项目目录：
   ```bash
   cd TEA_siren
   ```
2. 发布为单文件 exe：
   ```bash
   dotnet publish -c Release -r win-x64 --self-contained true -o EXE -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=none -p:DebugSymbols=false
   ```
   或直接双击运行项目目录下的 `publish.bat`。  
3. 生成的可执行文件位于 `EXE\TEA_siren_v3.6.1.exe`（版本号以 csproj 中 `Version` 为准）。

---

## 安全说明

- 本软件**完全免费**。如有人向您收费，请勿购买。  
- **行为范围**：仅会执行「选择目录 → 下载 ASI 文件 → 复制到指定 plugins 目录」，不会修改内存、不注入进程、不扫盘、无后门、无捆绑。  
- 关闭程序即完全退出，不会驻留后台。  
- 源码公开，可自行审查；若发现异常，请警惕是否为他人二次修改后重新发布的版本。

---

## 致谢与许可

- 插件本体来自 [KevinL852/SirenSetting_Limit_Adjuster](https://github.com/KevinL852/SirenSetting_Limit_Adjuster)，本仓库仅为安装器。  
- 本安装器 **制作人：TEARLESSVVOID** · © Copyright TEARLESSVVOID  
- 本仓库采用 [GPL-3.0](LICENSE) 许可证。
