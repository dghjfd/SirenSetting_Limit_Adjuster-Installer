// ========== 程序范围（仅以下功能，无其他行为） ==========
// · 选择/扫描 FiveM plugins 目录
// · 从 GitHub 指定地址下载 ASI 文件（仅 HttpClient GET）
// · 启动时检查本安装器在 GitHub 是否有新版本并提示
// · 备份旧版本到子文件夹、复制新文件到 plugins 目录
// · 无修改内存、无注入进程、无注册表、无自启动、无扫盘、无其他网络请求
// =========================================================

using System;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace TEA_siren;

public partial class MainForm : Form
{
    private const string AsiFileName = "SirenSetting_Limit_Adjuster_b3751.asi";
    private const int NewVersion = 3751;
    // Release 无独立资产，ASI 在 main 分支，优先 raw 链接，失败时从 zip 提取
    private static readonly string[] DownloadUrls =
    {
        "https://raw.githubusercontent.com/KevinL852/SirenSetting_Limit_Adjuster/main/SirenSetting_Limit_Adjuster_b3751.asi",
        "https://github.com/KevinL852/SirenSetting_Limit_Adjuster/archive/refs/heads/main.zip"
    };
    private static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(60)
    };

    private const string RepoReleasesLatestUrl = "https://api.github.com/repos/dghjfd/SirenSetting_Limit_Adjuster-Installer/releases/latest";

    public MainForm()
    {
        InitializeComponent();
        HttpClient.DefaultRequestHeaders.Add("User-Agent", "TEA_siren/1.0");
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        var ver = Assembly.GetExecutingAssembly().GetName().Version;
        Text = $"TEA Siren Installer v{ver?.Major ?? 3}.{ver?.Minor ?? 0}.{ver?.Build ?? 0} © TEARLESSVVOID";
        txtPath.Text = string.Empty;
        TrySetIconFromLogo();
        Log("程序已启动");
        BeginInvoke(ShowWelcomeMessage);
        _ = CheckForUpdateAsync();
    }

    private async Task CheckForUpdateAsync()
    {
        try
        {
            var current = Assembly.GetExecutingAssembly().GetName().Version;
            var currentTuple = (current?.Major ?? 3, current?.Minor ?? 0, current?.Build ?? 0);
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(10) };
            client.DefaultRequestHeaders.Add("User-Agent", "TEA_siren");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            var resp = await client.GetAsync(RepoReleasesLatestUrl).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode)
                return;
            var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            var release = JsonSerializer.Deserialize<GitHubRelease>(json);
            if (release?.tag_name == null)
                return;
            var remote = ParseVersionTag(release.tag_name);
            if (remote == null || CompareVersion(remote.Value, currentTuple) <= 0)
                return;
            var versionStr = release.tag_name.TrimStart('v');
            var body = string.IsNullOrWhiteSpace(release.body) ? "（无更新说明）" : release.body.Trim();
            var url = release.html_url ?? "https://github.com/dghjfd/SirenSetting_Limit_Adjuster-Installer/releases";
            BeginInvoke(() => ShowUpdateAvailable(this, versionStr, body, url));
        }
        catch
        {
            // 网络或解析失败时静默忽略，不影响使用
        }
    }

    private static (int major, int minor, int build)? ParseVersionTag(string tag)
    {
        if (string.IsNullOrWhiteSpace(tag))
            return null;
        var s = tag.TrimStart('v', 'V').Trim();
        var parts = s.Split('.');
        if (parts.Length < 2)
            return null;
        if (!int.TryParse(parts[0], out var major) || !int.TryParse(parts[1], out var minor))
            return null;
        var build = parts.Length >= 3 && int.TryParse(parts[2], out var b) ? b : 0;
        return (major, minor, build);
    }

    private static int CompareVersion((int major, int minor, int build) a, (int major, int minor, int build) b)
    {
        if (a.major != b.major) return a.major.CompareTo(b.major);
        if (a.minor != b.minor) return a.minor.CompareTo(b.minor);
        return a.build.CompareTo(b.build);
    }

    private static void ShowUpdateAvailable(Form owner, string versionStr, string releaseBody, string releaseUrl)
    {
        using var form = new Form
        {
            Text = "发现新版本",
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Size = new Size(480, 380),
            StartPosition = FormStartPosition.CenterParent,
            MaximizeBox = false,
            MinimizeBox = true,
            Owner = owner
        };
        var lblVer = new Label
        {
            Text = $"发现新版本：v{versionStr}，请前往 GitHub 下载。",
            AutoSize = true,
            Location = new Point(12, 12),
            Font = new Font(form.Font.FontFamily, 10f, FontStyle.Bold)
        };
        var lblLog = new Label { Text = "更新日志：", AutoSize = true, Location = new Point(12, 40) };
        var txtBody = new TextBox
        {
            ReadOnly = true,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            WordWrap = true,
            Location = new Point(12, 58),
            Size = new Size(440, 220),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = SystemColors.Window,
            Text = releaseBody
        };
        var btnOpen = new Button
        {
            Text = "前往下载",
            Size = new Size(100, 28),
            Location = new Point(12, 290)
        };
        btnOpen.Click += (_, _) =>
        {
            try
            {
                Process.Start(new ProcessStartInfo(releaseUrl) { UseShellExecute = true });
            }
            catch { /* ignore */ }
        };
        var btnClose = new Button
        {
            Text = "关闭",
            Size = new Size(100, 28),
            Location = new Point(120, 290)
        };
        btnClose.Click += (_, _) => form.Close();
        form.Controls.Add(lblVer);
        form.Controls.Add(lblLog);
        form.Controls.Add(txtBody);
        form.Controls.Add(btnOpen);
        form.Controls.Add(btnClose);
        form.ActiveControl = btnClose;
        form.ShowDialog();
    }

    private sealed class GitHubRelease
    {
        public string? tag_name { get; set; }
        public string? name { get; set; }
        public string? body { get; set; }
        public string? html_url { get; set; }
    }

    private static void ShowWelcomeMessage()
    {
        MessageBox.Show(
            "【SirenSetting Limit Adjuster 安装器】\n\n" +
            "▶ 功能：自动从 GitHub 官方仓库下载 ASI 插件到您指定的 FiveM plugins 目录，用于提升警笛相关限制。\n\n" +
            "▶ 使用方法：\n" +
            "  1. 点击「自动扫描」或「选择目录」指定 FiveM 的 plugins 文件夹\n" +
            "  2. 点击「安装插件」即可自动下载并安装\n" +
            "  3. 若已存在旧版本，将自动备份后替换\n\n" +
            "▶ 本软件完全免费。如有人向您收费，您已被骗，请勿购买。\n\n" +
            "▶ 安全声明：\n" +
            "  · 无后门、无捆绑、无扫盘，不修改内存、不注入进程\n" +
            "  · 仅执行：选择目录 → 下载 ASI 文件 → 复制到指定 plugins 目录\n" +
            "  · 关闭程序即完全退出，不会驻留后台\n" +
            "  · 源码可公开审核，欢迎监督。如有后门，那么你下载的程序可能遭到恶意二次修改并且重新宣传\n\n" +
            "© Copyright TEARLESSVVOID",
            "使用说明",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private Icon? _formIcon; // 保持引用，避免图标被回收后失效

    private void TrySetIconFromLogo()
    {
        try
        {
            if (TryGetIconFromExe(out var exeIcon))
            {
                _formIcon = exeIcon;
                Icon = _formIcon;
                return;
            }
            var asm = typeof(MainForm).Assembly;
            var name = asm.GetManifestResourceNames().FirstOrDefault(n => n.EndsWith("Logo.png", StringComparison.OrdinalIgnoreCase));
            if (name != null)
            {
                using var stream = asm.GetManifestResourceStream(name);
                if (stream != null)
                {
                    var bmp = new Bitmap(stream);
                    _formIcon = (Icon)Icon.FromHandle(bmp.GetHicon()).Clone();
                    bmp.Dispose();
                    Icon = _formIcon;
                    return;
                }
            }
            var exeDir = Path.GetDirectoryName(Application.ExecutablePath);
            if (!string.IsNullOrEmpty(exeDir))
            {
                var icoPath = Path.Combine(exeDir, "app.ico");
                if (File.Exists(icoPath))
                {
                    _formIcon = new Icon(icoPath);
                    Icon = _formIcon;
                }
            }
        }
        catch { /* ignore */ }
    }

    private static int? ExtractVersion(string fileName)
    {
        var m = Regex.Match(fileName, @"SirenSetting_Limit_Adjuster_b(\d+)\.asi", RegexOptions.IgnoreCase);
        return m.Success && int.TryParse(m.Groups[1].Value, out var v) ? v : null;
    }

    private static bool TryGetIconFromExe(out Icon? icon)
    {
        icon = null;
        try
        {
            var path = Application.ExecutablePath;
            if (string.IsNullOrEmpty(path) || !File.Exists(path)) return false;
            icon = Icon.ExtractAssociatedIcon(path);
            return icon != null;
        }
        catch { return false; }
    }

    private void Log(string message)
    {
        var line = $"[{DateTime.Now:HH:mm:ss}] {message}";
        if (txtLog.InvokeRequired)
        {
            txtLog.Invoke(() =>
            {
                txtLog.AppendText(line + Environment.NewLine);
                txtLog.ScrollToCaret();
            });
        }
        else
        {
            txtLog.AppendText(line + Environment.NewLine);
            txtLog.ScrollToCaret();
        }
    }

    private void ReportProgress(int percent, string? status = null)
    {
        if (progressBar.InvokeRequired)
        {
            progressBar.Invoke(() => ReportProgress(percent, status));
            return;
        }
        progressBar.Style = ProgressBarStyle.Blocks;
        progressBar.Minimum = 0;
        progressBar.Maximum = 100;
        progressBar.Value = Math.Clamp(percent, 0, 100);
        if (status != null)
            lblStatus.Text = status;
    }

    private void BtnBrowse_Click(object sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "选择 FiveM 的 plugins 目录（或 FiveM.app 目录）",
            UseDescriptionForTitle = true
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            var path = dialog.SelectedPath;
            if (path.EndsWith("plugins", StringComparison.OrdinalIgnoreCase))
            {
                txtPath.Text = path;
            }
            else if (path.EndsWith("FiveM.app", StringComparison.OrdinalIgnoreCase))
            {
                txtPath.Text = Path.Combine(path, "plugins");
            }
            else
            {
                txtPath.Text = Path.Combine(path, "plugins");
            }
            Log($"已选择目录: {txtPath.Text}");
        }
    }

    private void BtnScan_Click(object sender, EventArgs e)
    {
        btnScan.Enabled = false;
        lblStatus.Text = "正在扫描...";
        lblStatus.ForeColor = Color.FromArgb(140, 150, 160);
        Log("开始扫描 FiveM 安装目录...");
        Application.DoEvents();

        var found = ScanForFiveMPlugins();

        btnScan.Enabled = true;
        if (found != null)
        {
            txtPath.Text = found;
            lblStatus.Text = "已找到";
            lblStatus.ForeColor = Color.FromArgb(120, 200, 140);
            Log($"扫描成功: {found}");
        }
        else
        {
            lblStatus.Text = "未找到";
            lblStatus.ForeColor = Color.FromArgb(220, 180, 100);
            Log("未找到 FiveM 安装目录");
        }
    }

    private static string? ScanForFiveMPlugins()
    {
        static string? PluginsPath(string appPath)
        {
            if (Directory.Exists(appPath))
                return Path.Combine(appPath, "plugins");
            return null;
        }

        var searchPaths = new[]
        {
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "FiveM", "FiveM.app"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "FiveM", "FiveM.app"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "FiveM", "FiveM.app"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "FiveM", "FiveM.app"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "FiveM", "FiveM.app")
        };

        foreach (var basePath in searchPaths)
        {
            var plugins = PluginsPath(basePath);
            if (plugins != null) return plugins;
        }

        var driveLetters = new[] { "C", "D", "E", "F", "G", "H" };
        foreach (var letter in driveLetters)
        {
            var root = letter + ":\\";
            if (!Directory.Exists(root)) continue;

            var direct = Path.Combine(root, "FiveM", "FiveM.app");
            var plugins = PluginsPath(direct);
            if (plugins != null) return plugins;

            var atRoot = Path.Combine(root, "FiveM.app");
            plugins = PluginsPath(atRoot);
            if (plugins != null) return plugins;

            try
            {
                foreach (var top in Directory.GetDirectories(root))
                {
                    var appPath = Path.Combine(top, "FiveM.app");
                    plugins = PluginsPath(appPath);
                    if (plugins != null) return plugins;

                    appPath = Path.Combine(top, "FiveM", "FiveM.app");
                    plugins = PluginsPath(appPath);
                    if (plugins != null) return plugins;
                }
            }
            catch { /* 无权限等忽略 */ }
        }

        foreach (var drive in DriveInfo.GetDrives())
        {
            if (!drive.IsReady || drive.DriveType != DriveType.Fixed) continue;
            var path = Path.Combine(drive.Name.TrimEnd('\\'), "FiveM", "FiveM.app");
            var plugins = PluginsPath(path);
            if (plugins != null) return plugins;
        }

        var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var fivemFolder = Path.Combine(localAppData, "FiveM");
        if (Directory.Exists(fivemFolder))
        {
            foreach (var dir in Directory.GetDirectories(fivemFolder))
            {
                var appPath = Path.Combine(dir, "FiveM.app");
                if (Directory.Exists(appPath))
                    return Path.Combine(appPath, "plugins");
            }
        }

        return null;
    }

    private async void BtnInstall_Click(object sender, EventArgs e)
    {
        var pluginsPath = txtPath.Text.Trim();
        if (string.IsNullOrEmpty(pluginsPath))
        {
            Log("错误: 请先选择或扫描 plugins 目录");
            MessageBox.Show("请先选择或扫描 plugins 目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnInstall.Enabled = false;
        btnBrowse.Enabled = false;
        btnScan.Enabled = false;
        progressBar.Minimum = 0;
        progressBar.Maximum = 100;
        progressBar.Value = 0;
        progressBar.Style = ProgressBarStyle.Blocks;
        Log("开始安装...");
        Application.DoEvents();

        try
        {
            await InstallPluginAsync(pluginsPath);
        }
        catch (Exception ex)
        {
            lblStatus.Text = "失败";
            lblStatus.ForeColor = Color.FromArgb(220, 100, 100);
            Log($"安装失败: {ex.Message}");
            MessageBox.Show($"安装失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            btnInstall.Enabled = true;
            btnBrowse.Enabled = true;
            btnScan.Enabled = true;
        }
    }

    private async Task<byte[]> DownloadAsiAsync()
    {
        ReportProgress(5, "正在下载...");
        Log("正在下载 ASI 文件...");
        var response = await HttpClient.GetAsync(DownloadUrls[0]);
        if (response.IsSuccessStatusCode)
        {
            ReportProgress(35, "下载完成");
            return await response.Content.ReadAsByteArrayAsync();
        }

        ReportProgress(15, "从压缩包提取...");
        Log("直链失败，尝试从仓库压缩包提取...");
        var zipBytes = await HttpClient.GetByteArrayAsync(DownloadUrls[1]);
        using var zip = new ZipArchive(new MemoryStream(zipBytes), ZipArchiveMode.Read);
        var entry = zip.GetEntry($"SirenSetting_Limit_Adjuster-main/{AsiFileName}")
                   ?? zip.GetEntry(AsiFileName);
        if (entry == null)
        {
            foreach (var e in zip.Entries)
            {
                if (e.Name.EndsWith(".asi", StringComparison.OrdinalIgnoreCase))
                {
                    entry = e;
                    break;
                }
            }
        }
        if (entry == null)
            throw new Exception("压缩包中未找到 ASI 文件");

        await using var stream = entry.Open();
        using var ms = new MemoryStream();
        await stream.CopyToAsync(ms);
        ReportProgress(35, "下载完成");
        return ms.ToArray();
    }

    private async Task InstallPluginAsync(string pluginsPath)
    {
        ReportProgress(0, "准备中...");
        Log($"目标目录: {pluginsPath}");

        if (!Directory.Exists(pluginsPath))
        {
            Directory.CreateDirectory(pluginsPath);
            Log($"已创建 plugins 目录");
        }

        var tempFile = Path.Combine(Path.GetTempPath(), AsiFileName);

        try
        {
            var bytes = await DownloadAsiAsync();
            await File.WriteAllBytesAsync(tempFile, bytes);
            Log($"下载完成 ({bytes.Length / 1024} KB)");

            ReportProgress(40, "检查现有版本...");
            var destPath = Path.Combine(pluginsPath, AsiFileName);
            var existingFiles = Directory.Exists(pluginsPath)
                ? Directory.GetFiles(pluginsPath, "SirenSetting_Limit_Adjuster_b*.asi")
                : Array.Empty<string>();
            var toBackup = existingFiles
                .Select(f => (Path: f, Ver: ExtractVersion(Path.GetFileName(f))))
                .Where(x => x.Ver.HasValue && x.Ver.Value < NewVersion)
                .ToList();

            if (toBackup.Count > 0 || File.Exists(destPath))
            {
                ReportProgress(42, "等待确认...");
                var msg = toBackup.Count > 0
                    ? $"检测到 {toBackup.Count} 个旧版本（版本号 < b{NewVersion}）\n将全部备份后安装新版本，是否继续？"
                    : $"目标位置已存在 b{NewVersion}，是否替换？\n（旧文件将移入备份文件夹）";
                var result = MessageBox.Show(msg, "确认替换", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes)
                {
                    Log("用户取消替换，保留原文件");
                    ReportProgress(0, "已取消");
                    lblStatus.ForeColor = Color.FromArgb(140, 150, 160);
                    return;
                }
                Log("用户确认替换");
            }

            ReportProgress(50, "备份旧版本...");
            var backupFolder = Path.Combine(pluginsPath, "SirenSetting_Limit_Adjuster_备份");
            if (!Directory.Exists(backupFolder))
            {
                Directory.CreateDirectory(backupFolder);
                Log($"已创建备份文件夹: {backupFolder}");
            }

            var step = toBackup.Count > 0 ? 25 / Math.Max(1, toBackup.Count + 1) : 0;
            var p = 50;
            foreach (var (filePath, _) in toBackup)
            {
                var name = Path.GetFileName(filePath);
                var backupPath = Path.Combine(backupFolder, name);
                File.Copy(filePath, backupPath, overwrite: true);
                File.Delete(filePath);
                p += step;
                ReportProgress(p, "备份旧版本...");
                Log($"已备份并删除旧版本: {name}");
            }

            if (File.Exists(destPath) && !toBackup.Any(o => string.Equals(o.Path, destPath, StringComparison.OrdinalIgnoreCase)))
            {
                var backupPath = Path.Combine(backupFolder, Path.GetFileName(destPath));
                File.Copy(destPath, backupPath, overwrite: true);
                File.Delete(destPath);
                ReportProgress(75, "备份完成");
                Log($"已备份并删除: {Path.GetFileName(destPath)}");
            }
            else if (toBackup.Count > 0)
            {
                ReportProgress(75, "备份完成");
            }

            ReportProgress(85, "安装中...");
            File.Copy(tempFile, destPath, overwrite: true);
            Log($"已安装新版本 (b{NewVersion}): {destPath}");

            ReportProgress(100, "成功");
            lblStatus.ForeColor = Color.FromArgb(120, 200, 140);
            MessageBox.Show($"SirenSetting Limit Adjuster 已成功安装到:\n{destPath}", "安装完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Log("安装完成");
        }
        finally
        {
            if (File.Exists(tempFile))
            {
                try
                {
                    File.Delete(tempFile);
                    Log("已清理临时文件");
                }
                catch { /* ignore */ }
            }
        }
    }
}
