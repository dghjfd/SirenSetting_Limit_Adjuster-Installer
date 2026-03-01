namespace TEA_siren;

/// <summary>
/// UI strings: default EN, optional 中文.
/// </summary>
public static class Lang
{
    public static string Current { get; set; } = "en";

    private static readonly Dictionary<string, Dictionary<string, string>> Strings = new()
    {
        ["en"] = new Dictionary<string, string>
        {
            ["FormTitle"] = "TEA Siren Installer",
            ["TabInstall"] = "Install",
            ["TabOptions"] = "Options",
            ["TabSponsors"] = "Sponsors",
            ["TabChangelog"] = "Changelog",
            ["Title"] = "SirenSetting Limit Adjuster Installer",
            ["PluginsDir"] = "Plugins directory:",
            ["PathPlaceholder"] = "Select or scan directory...",
            ["BtnBrowse"] = "Browse",
            ["BtnScan"] = "Auto scan",
            ["BtnInstall"] = "Install plugin",
            ["BtnInstallLocal"] = "Install from local",
            ["BtnCheckUpdate"] = "Check update",
            ["StatusReady"] = "Ready",
            ["LblLog"] = "Log:",
            ["Copyright"] = "© TEARLESSVVOID",
            ["OptionLanguage"] = "Language:",
            ["OptionCheckOnStartup"] = "Check for update on startup",
            ["SponsorsTitle"] = "Sponsors & Support",
            ["SponsorsBody"] = "Thanks to everyone who supports this project.\n\n(Sponsor list can be updated in the app.)",
            ["ChangelogTitle"] = "Update history",
            ["ChangelogLoad"] = "Load from GitHub",
            ["ChangelogLoading"] = "Loading...",
            ["WelcomeTitle"] = "Usage",
            ["WelcomeBody"] = "【SirenSetting Limit Adjuster Installer】\n\n► Features: Download ASI from GitHub to your FiveM plugins folder.\n\n► Usage:\n  1. Click \"Auto scan\" or \"Browse\" to set the plugins folder\n  2. Click \"Install plugin\" or \"Install from local\" if you have the .asi file\n  3. Old versions are backed up before install.\n\n► Free software. Do not pay anyone for it.\n\n► Safety: No backdoor, no bundling, no disk scan. Source is public.\n\nAuthor: TEARLESSVVOID · © Copyright TEARLESSVVOID",
        },
        ["zh"] = new Dictionary<string, string>
        {
            ["FormTitle"] = "TEA Siren 安装器",
            ["TabInstall"] = "安装",
            ["TabOptions"] = "选项",
            ["TabSponsors"] = "赞助",
            ["TabChangelog"] = "更新日志",
            ["Title"] = "SirenSetting Limit Adjuster 安装器",
            ["PluginsDir"] = "Plugins 目录:",
            ["PathPlaceholder"] = "选择或扫描目录...",
            ["BtnBrowse"] = "选择目录",
            ["BtnScan"] = "自动扫描",
            ["BtnInstall"] = "安装插件",
            ["BtnInstallLocal"] = "从本地安装",
            ["BtnCheckUpdate"] = "检查更新",
            ["StatusReady"] = "就绪",
            ["LblLog"] = "日志:",
            ["Copyright"] = "© TEARLESSVVOID",
            ["OptionLanguage"] = "语言:",
            ["OptionCheckOnStartup"] = "启动时检查更新",
            ["SponsorsTitle"] = "赞助与支持",
            ["SponsorsBody"] = "感谢所有支持本项目的朋友。\n\n（赞助列表可在应用中更新。）",
            ["ChangelogTitle"] = "更新历史",
            ["ChangelogLoad"] = "从 GitHub 加载",
            ["ChangelogLoading"] = "加载中...",
            ["WelcomeTitle"] = "使用说明",
            ["WelcomeBody"] = "【SirenSetting Limit Adjuster 安装器】\n\n▶ 功能：从 GitHub 下载 ASI 到 FiveM plugins 目录。\n\n▶ 使用方法：\n  1. 点击「自动扫描」或「选择目录」指定 plugins 文件夹\n  2. 点击「安装插件」或「从本地安装」选择 .asi 文件\n  3. 安装前会自动备份旧版本。\n\n▶ 本软件完全免费，请勿向他人付费。\n\n▶ 安全：无后门、无捆绑、不扫盘，源码公开。\n\n制作人: TEARLESSVVOID · © Copyright TEARLESSVVOID",
        }
    };

    public static string Get(string key)
    {
        if (Strings.TryGetValue(Current, out var dict) && dict.TryGetValue(key, out var s))
            return s;
        if (Strings.TryGetValue("en", out var en) && en.TryGetValue(key, out var enS))
            return enS;
        return key;
    }
}
