# TEA Siren Installer

> FiveM 警灯上限插件（SirenSetting Limit Adjuster）一键安装工具 · 便携单文件 · 免费无捆绑  
> **English:** [README.md](README.md)

基于 [SirenSetting_Limit_Adjuster](https://github.com/KevinL852/SirenSetting_Limit_Adjuster) 的图形化安装器，将 ASI 安装到 FiveM 的 `plugins` 目录。**界面默认英文**，可在 **选项** 页切换为 中文。

---

## 功能特点

- **分页**：安装 · 选项 · 赞助 · 更新日志
- **语言**：英文（默认）、中文，选项内保存
- **自动扫描**：自动查找本机 FiveM `plugins` 目录
- **一键安装**：从 GitHub 下载最新 ASI（含国内代理备用）
- **从本地安装**：无法访问 GitHub 时可选本地 .asi 文件安装
- **旧版备份**：安装前自动备份
- **更新检查**：可选启动时检查；程序内「检查更新」；超过 2 个版本未更新则需强制更新
- **程序内更新**：下载新版本到 temp 并覆盖当前 exe（国内 CDN 或直连）
- **赞助**：页内致谢与赞助列表
- **更新日志**：页内从 GitHub 加载发布历史
- **单文件便携**：单 exe，无需单独 .NET 运行时

---

## 下载

- **Releases**：[Releases](https://github.com/dghjfd/SirenSetting_Limit_Adjuster-Installer/releases) 下载最新 `TEA_siren_v*.exe`
- 直接运行，无需安装。

---

## 使用方法

1. 运行 `TEA_siren_v*.exe`，首次会弹出使用说明（语言随选项）。
2. **安装** 页：选择 plugins 目录（自动扫描或选择目录），再点「安装插件」或「从本地安装」。
3. **选项** 页：设置 **语言**（English / 中文）、**启动时检查更新**。
4. **赞助** 页：查看致谢与赞助列表。
5. **更新日志** 页：点击「从 GitHub 加载」获取发布历史。
6. 安装完成后重启或启动 FiveM。

---

## 从源码编译

- **环境**：[.NET 8 SDK](https://dotnet.microsoft.com/download)、Windows
- **步骤**：
  ```bash
  cd TEA_siren
  dotnet publish -c Release -r win-x64 --self-contained true -o EXE -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:EnableCompressionInSingleFile=true -p:DebugType=none -p:DebugSymbols=false
  ```
  或运行 `publish.bat`。输出：`EXE\TEA_siren_v*.exe`。

---

## 安全说明

- 本软件**完全免费**，请勿向他人付费。
- 仅会：选择目录、下载 ASI、复制到 plugins。无后门、无捆绑、不扫盘、不修改内存/进程。关闭即退出。
- 源码公开可审；请从官方 Release 获取，避免二次修改版。

---

## 致谢与许可

- 插件来自 [KevinL852/SirenSetting_Limit_Adjuster](https://github.com/KevinL852/SirenSetting_Limit_Adjuster)，本仓库仅为安装器。
- **制作人**：TEARLESSVVOID · © Copyright TEARLESSVVOID
- 许可：[GPL-3.0](LICENSE)。
