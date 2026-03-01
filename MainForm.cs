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
    // Release 无独立资产，ASI 在 main 分支。优先官方 raw，失败则试国内代理（实时转发 GitHub），最后从 zip 提取
    private const string RawAsiUrl = "https://raw.githubusercontent.com/KevinL852/SirenSetting_Limit_Adjuster/main/SirenSetting_Limit_Adjuster_b3751.asi";
    private static readonly string[] RawProxyPrefixes = { "https://mirror.ghproxy.com/", "https://ghproxy.net/" };
    private static readonly string ZipArchiveUrl = "https://github.com/KevinL852/SirenSetting_Limit_Adjuster/archive/refs/heads/main.zip";
    private static readonly HttpClient HttpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(60)
    };

    private const string RepoReleasesUrl = "https://api.github.com/repos/dghjfd/SirenSetting_Limit_Adjuster-Installer/releases?per_page=20";

    private static string SettingsPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "TEA_siren", "settings.txt");

    public MainForm()
    {
        InitializeComponent();
        HttpClient.DefaultRequestHeaders.Add("User-Agent", "TEA_siren/1.0");
    }

    private void MainForm_Load(object sender, EventArgs e)
    {
        var ver = Assembly.GetExecutingAssembly().GetName().Version;
        Text = $"{Lang.Get("FormTitle")} v{ver?.Major ?? 3}.{ver?.Minor ?? 0}.{ver?.Build ?? 0} © TEARLESSVVOID";
        txtPath.Text = string.Empty;
        TrySetIconFromLogo();
        LoadSettings();
        ApplyLanguage();
        chkCheckUpdateStartup.CheckedChanged += (_, _) => SaveSettings();
        TrySetIconFromLogo();
        Log(Lang.Current == "zh" ? "程序已启动" : "Program started");
        BeginInvoke(ShowWelcomeMessage);
        if (chkCheckUpdateStartup.Checked)
            _ = CheckForUpdateAsync();
    }

    private void LoadSettings()
    {
        try
        {
            if (File.Exists(SettingsPath))
            {
                var lines = File.ReadAllLines(SettingsPath);
                foreach (var line in lines)
                {
                    var kv = line.Split(new[] { '=' }, 2, StringSplitOptions.None);
                    if (kv.Length != 2) continue;
                    var k = kv[0].Trim();
                    var v = kv[1].Trim();
                    if (k == "lang" && (v == "en" || v == "zh")) Lang.Current = v;
                    if (k == "check_update_startup") chkCheckUpdateStartup.Checked = v != "0";
                }
            }
            else
            {
                Lang.Current = "en";
                chkCheckUpdateStartup.Checked = true;
            }
            comboLanguage.SelectedIndex = Lang.Current == "zh" ? 1 : 0;
        }
        catch { Lang.Current = "en"; comboLanguage.SelectedIndex = 0; chkCheckUpdateStartup.Checked = true; }
    }

    private void SaveSettings()
    {
        try
        {
            var dir = Path.GetDirectoryName(SettingsPath);
            if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);
            File.WriteAllText(SettingsPath, $"lang={Lang.Current}\r\ncheck_update_startup={(chkCheckUpdateStartup.Checked ? "1" : "0")}\r\n");
        }
        catch { }
    }

    private void ApplyLanguage()
    {
        var ver = Assembly.GetExecutingAssembly().GetName().Version;
        Text = $"{Lang.Get("FormTitle")} v{ver?.Major ?? 3}.{ver?.Minor ?? 0}.{ver?.Build ?? 0} © TEARLESSVVOID";
        tabInstall.Text = Lang.Get("TabInstall");
        tabOptions.Text = Lang.Get("TabOptions");
        tabSponsors.Text = Lang.Get("TabSponsors");
        tabChangelog.Text = Lang.Get("TabChangelog");
        lblTitle.Text = Lang.Get("Title");
        lblPath.Text = Lang.Get("PluginsDir");
        txtPath.PlaceholderText = Lang.Get("PathPlaceholder");
        btnBrowse.Text = Lang.Get("BtnBrowse");
        btnScan.Text = Lang.Get("BtnScan");
        btnInstall.Text = Lang.Get("BtnInstall");
        btnInstallLocal.Text = Lang.Get("BtnInstallLocal");
        btnCheckUpdate.Text = Lang.Get("BtnCheckUpdate");
        lblStatus.Text = Lang.Get("StatusReady");
        lblLog.Text = Lang.Get("LblLog");
        lblCopyright.Text = Lang.Get("Copyright");
        lblOptionLanguage.Text = Lang.Get("OptionLanguage");
        chkCheckUpdateStartup.Text = Lang.Get("OptionCheckOnStartup");
        lblSponsorsTitle.Text = Lang.Get("SponsorsTitle");
        txtSponsors.Text = Lang.Get("SponsorsBody");
        lblChangelogTitle.Text = Lang.Get("ChangelogTitle");
        btnChangelogLoad.Text = Lang.Get("ChangelogLoad");
    }

    private void ComboLanguage_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (comboLanguage.SelectedIndex < 0) return;
        Lang.Current = comboLanguage.SelectedIndex == 1 ? "zh" : "en";
        ApplyLanguage();
        SaveSettings();
    }

    private async void BtnChangelogLoad_Click(object? sender, EventArgs e)
    {
        btnChangelogLoad.Enabled = false;
        btnChangelogLoad.Text = Lang.Get("ChangelogLoading");
        txtChangelog.Text = "";
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
            client.DefaultRequestHeaders.Add("User-Agent", "TEA_siren");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            var resp = await client.GetAsync(RepoReleasesUrl).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode) { txtChangelog.Text = "Failed to load."; return; }
            var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            var releases = JsonSerializer.Deserialize<List<GitHubRelease>>(json);
            var sb = new System.Text.StringBuilder();
            if (releases != null)
                foreach (var r in releases)
                {
                    if (string.IsNullOrWhiteSpace(r.tag_name)) continue;
                    sb.Append("━━━ ").Append(r.tag_name).AppendLine(" ━━━");
                    sb.AppendLine(string.IsNullOrWhiteSpace(r.body) ? "(No description)" : r.body.Trim());
                    sb.AppendLine();
                }
            BeginInvoke(() => { txtChangelog.Text = sb.Length > 0 ? sb.ToString().TrimEnd() : "No releases."; });
        }
        catch { BeginInvoke(() => txtChangelog.Text = "Network error."); }
        finally { BeginInvoke(() => { btnChangelogLoad.Enabled = true; btnChangelogLoad.Text = Lang.Get("ChangelogLoad"); }); }
    }

    private const int MaxSkipVersions = 2;

    private async Task CheckForUpdateAsync()
    {
        var result = await FetchUpdateInfoAsync().ConfigureAwait(false);
        if (result == null || !result.Value.hasUpdate)
            return;
        var r = result.Value;
        var forceUpdate = r.skipCount > MaxSkipVersions;
        BeginInvoke(() => ShowUpdateAvailable(this, r.versionStr, r.fullChangelog, r.url, forceUpdate, r.exeDownloadUrl));
    }

    /// <summary>
    /// 获取 GitHub 发布列表，判断是否有更新，并拼接各版本更新日志。返回 null 表示网络/解析失败。
    /// skipCount = 当前版本与最新之间隔了多少个发布版本；超过 MaxSkipVersions 则强制更新。
    /// </summary>
    private async Task<(bool hasUpdate, string versionStr, string fullChangelog, string url, (int, int, int) currentTuple, string? exeDownloadUrl, int skipCount)?> FetchUpdateInfoAsync()
    {
        try
        {
            var current = Assembly.GetExecutingAssembly().GetName().Version;
            var currentTuple = (current?.Major ?? 3, current?.Minor ?? 0, current?.Build ?? 0);
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
            client.DefaultRequestHeaders.Add("User-Agent", "TEA_siren");
            client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
            var resp = await client.GetAsync(RepoReleasesUrl).ConfigureAwait(false);
            if (!resp.IsSuccessStatusCode)
                return (false, "", "", "", currentTuple, null, 0);
            var json = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
            var releases = JsonSerializer.Deserialize<List<GitHubRelease>>(json);
            if (releases == null || releases.Count == 0)
                return (false, "", "", "", currentTuple, null, 0);
            var latest = releases[0];
            if (latest.tag_name == null)
                return (false, "", "", "", currentTuple, null, 0);
            var remote = ParseVersionTag(latest.tag_name);
            if (remote == null || CompareVersion(remote.Value, currentTuple) <= 0)
                return (false, "", "", "", currentTuple, null, 0);
            int skipCount = 0;
            foreach (var rel in releases)
            {
                var v = ParseVersionTag(rel.tag_name ?? "");
                if (v.HasValue && CompareVersion(v.Value, currentTuple) > 0)
                    skipCount++;
                else
                    break;
            }
            var versionStr = latest.tag_name.TrimStart('v');
            var url = latest.html_url ?? "https://github.com/dghjfd/SirenSetting_Limit_Adjuster-Installer/releases";
            var sb = new System.Text.StringBuilder();
            foreach (var r in releases)
            {
                if (string.IsNullOrWhiteSpace(r.tag_name)) continue;
                sb.Append("━━━ ").Append(r.tag_name).AppendLine(" ━━━");
                sb.AppendLine(string.IsNullOrWhiteSpace(r.body) ? "（无更新说明）" : r.body.Trim());
                sb.AppendLine();
            }
            var fullChangelog = sb.ToString().TrimEnd();
            string? exeDownloadUrl = null;
            if (latest.assets != null)
            {
                var exeAsset = latest.assets.FirstOrDefault(a => a?.name?.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) == true);
                exeDownloadUrl = exeAsset?.browser_download_url;
            }
            return (true, versionStr, fullChangelog, url, currentTuple, exeDownloadUrl, skipCount);
        }
        catch
        {
            return null;
        }
    }

    private async void BtnCheckUpdate_Click(object? sender, EventArgs e)
    {
        btnCheckUpdate.Enabled = false;
        lblStatus.Text = "正在检查...";
        lblStatus.ForeColor = Color.FromArgb(170, 178, 188);
        Log("用户手动检查更新");
        var result = await FetchUpdateInfoAsync().ConfigureAwait(false);
        BeginInvoke(() =>
        {
            btnCheckUpdate.Enabled = true;
            if (result == null)
            {
                lblStatus.Text = "就绪";
                Log("检查更新失败（网络或解析错误）");
                MessageBox.Show("检查更新失败，请检查网络连接或稍后再试。", "检查更新", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (result.Value.hasUpdate)
            {
                var r = result.Value;
                var forceUpdate = r.skipCount > MaxSkipVersions;
                ShowUpdateAvailable(this, r.versionStr, r.fullChangelog, r.url, forceUpdate, r.exeDownloadUrl);
            }
            else
            {
                lblStatus.Text = "就绪";
                lblStatus.ForeColor = Color.FromArgb(120, 200, 140);
                Log("当前已是最新版本");
                MessageBox.Show("当前已是最新版本，无需更新。", "检查更新", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        });
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

    private static void ShowUpdateAvailable(Form owner, string versionStr, string fullChangelog, string releaseUrl, bool forceUpdate, string? exeDownloadUrl)
    {
        using var form = new Form
        {
            Text = forceUpdate ? "需要更新" : "发现新版本",
            FormBorderStyle = FormBorderStyle.FixedDialog,
            Size = new Size(520, 420),
            StartPosition = FormStartPosition.CenterParent,
            MaximizeBox = false,
            MinimizeBox = !forceUpdate,
            Owner = owner
        };
        var lblVer = new Label
        {
            Text = forceUpdate
                ? $"检测到新版本 v{versionStr}，请更新后再使用本程序。"
                : $"发现新版本：v{versionStr}，可使用下方方式更新。",
            AutoSize = true,
            Location = new Point(12, 12),
            Font = new Font(form.Font.FontFamily, 10f, FontStyle.Bold),
            ForeColor = forceUpdate ? Color.FromArgb(200, 80, 80) : form.ForeColor
        };
        var lblLog = new Label { Text = "各版本更新日志：", AutoSize = true, Location = new Point(12, 40) };
        var txtBody = new TextBox
        {
            ReadOnly = true,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical,
            WordWrap = true,
            Location = new Point(12, 58),
            Size = new Size(480, 200),
            BorderStyle = BorderStyle.FixedSingle,
            BackColor = SystemColors.Window,
            Text = fullChangelog
        };
        var yBtn = 268;
        var btnUpdate = new Button
        {
            Text = "立即更新",
            Size = new Size(100, 28),
            Location = new Point(12, yBtn)
        };
        btnUpdate.Enabled = !string.IsNullOrEmpty(exeDownloadUrl);
        btnUpdate.Click += (_, _) =>
        {
            if (string.IsNullOrEmpty(exeDownloadUrl)) return;
            var currentExe = Application.ExecutablePath ?? "";
            if (string.IsNullOrEmpty(currentExe))
            {
                MessageBox.Show("无法获取当前程序路径。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            btnUpdate.Enabled = false;
            _ = DownloadAndReplaceSelfAsync(exeDownloadUrl, currentExe, form, () => btnUpdate.Enabled = true);
        };
        var btnWeb = new Button
        {
            Text = "前往官网",
            Size = new Size(88, 28),
            Location = new Point(118, yBtn)
        };
        btnWeb.Click += (_, _) =>
        {
            try { Process.Start(new ProcessStartInfo(releaseUrl) { UseShellExecute = true }); } catch { }
            form.Close();
        };
        if (forceUpdate)
            form.FormClosing += (_, _) => Application.Exit();
        form.Controls.Add(lblVer);
        form.Controls.Add(lblLog);
        form.Controls.Add(txtBody);
        form.Controls.Add(btnUpdate);
        form.Controls.Add(btnWeb);
        if (!forceUpdate)
        {
            var btnClose = new Button { Text = "关闭", Size = new Size(88, 28), Location = new Point(212, yBtn) };
            btnClose.Click += (_, _) => form.Close();
            form.Controls.Add(btnClose);
            form.ActiveControl = btnClose;
        }
        else
            form.ActiveControl = btnUpdate;
        form.ShowDialog();
        if (forceUpdate)
            Application.Exit();
    }

    /// <summary>
    /// 下载新版本到 temp，退出后由批处理覆盖当前 exe 并启动新版本。
    /// 先试国内 CDN（代理），失败再试直连。
    /// </summary>
    private static async Task DownloadAndReplaceSelfAsync(string exeDownloadUrl, string currentExePath, Form parentForm, Action? onFailure = null)
    {
        var tempDir = Path.GetTempPath();
        var tempExe = Path.Combine(tempDir, "TEA_siren_update_" + DateTime.Now.Ticks + ".exe");
        byte[]? bytes = null;
        try
        {
            using var client = new HttpClient { Timeout = TimeSpan.FromSeconds(90) };
            client.DefaultRequestHeaders.Add("User-Agent", "TEA_siren");
            foreach (var baseUrl in new[] { RawProxyPrefixes[0] + exeDownloadUrl, exeDownloadUrl })
            {
                try
                {
                    bytes = await client.GetByteArrayAsync(baseUrl).ConfigureAwait(false);
                    if (bytes != null && bytes.Length > 1000) break;
                }
                catch { /* 试下一个 */ }
            }
            if (bytes == null || bytes.Length < 1000)
            {
                parentForm.Invoke(() =>
                {
                    onFailure?.Invoke();
                    MessageBox.Show("下载失败（国内CDN 与直连均不可用）。请检查网络或稍后点击「立即更新」，或前往官网下载。", "下载失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                });
                return;
            }
            await File.WriteAllBytesAsync(tempExe, bytes).ConfigureAwait(false);
            var batPath = Path.Combine(tempDir, "TEA_siren_replace_" + DateTime.Now.Ticks + ".bat");
            var batContent = "@echo off\r\nchcp 65001 >nul\r\ntimeout /t 2 /nobreak >nul\r\ncopy /Y \"" + tempExe.Replace("\"", "\"\"") + "\" \"" + currentExePath.Replace("\"", "\"\"") + "\"\r\nstart \"\" \"" + currentExePath.Replace("\"", "\"\"") + "\"\r\ndel \"%~f0\"\r\n";
            await File.WriteAllTextAsync(batPath, batContent, System.Text.Encoding.ASCII).ConfigureAwait(false);
            parentForm.Invoke(() =>
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = "/c \"" + batPath + "\"",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });
                    parentForm.Close();
                    Application.Exit();
                }
                catch (Exception ex)
                {
                    onFailure?.Invoke();
                    MessageBox.Show($"无法启动更新：{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            });
        }
        catch (Exception ex)
        {
            parentForm.Invoke(() =>
            {
                onFailure?.Invoke();
                MessageBox.Show($"下载或写入失败：{ex.Message}\n请检查网络后重试，或前往官网下载。", "下载失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            });
        }
    }

    private sealed class GitHubRelease
    {
        public string? tag_name { get; set; }
        public string? name { get; set; }
        public string? body { get; set; }
        public string? html_url { get; set; }
        public GitHubReleaseAsset[]? assets { get; set; }
    }

    private sealed class GitHubReleaseAsset
    {
        public string? name { get; set; }
        public string? browser_download_url { get; set; }
    }

    private static void ShowWelcomeMessage()
    {
        MessageBox.Show(Lang.Get("WelcomeBody"), Lang.Get("WelcomeTitle"), MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        btnInstallLocal.Enabled = false;
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
            var msg = ex.Message;
            var isNetworkError = msg.Contains("raw.githubusercontent.com", StringComparison.OrdinalIgnoreCase)
                || msg.Contains("找不到请求的类型的数据", StringComparison.OrdinalIgnoreCase)
                || msg.Contains("无法连接", StringComparison.OrdinalIgnoreCase)
                || ex is HttpRequestException || ex is System.Net.Sockets.SocketException;
            if (isNetworkError)
            {
                MessageBox.Show(
                    "无法连接 GitHub（当前网络可能无法访问，如遇墙或 DNS 异常）。\n\n" +
                    "请改用「从本地安装」：\n" +
                    "1. 通过代理、镜像或他人获取 " + AsiFileName + "\n" +
                    "2. 点击「从本地安装」并选择该文件即可完成安装。",
                    "无法连接 GitHub",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show($"安装失败: {msg}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        finally
        {
            btnInstall.Enabled = true;
            btnInstallLocal.Enabled = true;
            btnBrowse.Enabled = true;
            btnScan.Enabled = true;
        }
    }

    private void BtnInstallLocal_Click(object sender, EventArgs e)
    {
        var pluginsPath = txtPath.Text.Trim();
        if (string.IsNullOrEmpty(pluginsPath))
        {
            Log("错误: 请先选择或扫描 plugins 目录");
            MessageBox.Show("请先选择或扫描 plugins 目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        using var dialog = new OpenFileDialog
        {
            Title = "选择已下载的 ASI 文件（无法访问 GitHub 时可用）",
            Filter = "ASI 文件 (*.asi)|*.asi|所有文件 (*.*)|*.*",
            FilterIndex = 1
        };
        if (dialog.ShowDialog() != DialogResult.OK)
            return;

        var sourcePath = dialog.FileName;
        var destFileName = Path.GetFileName(sourcePath);
        if (string.IsNullOrEmpty(destFileName) || !destFileName.EndsWith(".asi", StringComparison.OrdinalIgnoreCase))
        {
            MessageBox.Show("请选择有效的 .asi 文件。", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        btnInstall.Enabled = false;
        btnInstallLocal.Enabled = false;
        btnBrowse.Enabled = false;
        btnScan.Enabled = false;
        progressBar.Minimum = 0;
        progressBar.Maximum = 100;
        progressBar.Value = 0;
        progressBar.Style = ProgressBarStyle.Blocks;
        Log("开始从本地安装...");
        Application.DoEvents();

        try
        {
            InstallFromLocalFile(pluginsPath, sourcePath, destFileName);
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
            btnInstallLocal.Enabled = true;
            btnBrowse.Enabled = true;
            btnScan.Enabled = true;
        }
    }

    private void InstallFromLocalFile(string pluginsPath, string sourcePath, string destFileName)
    {
        ReportProgress(5, "准备中...");
        Log($"目标目录: {pluginsPath}");
        Log($"源文件: {sourcePath}");

        if (!Directory.Exists(pluginsPath))
        {
            Directory.CreateDirectory(pluginsPath);
            Log("已创建 plugins 目录");
        }

        var fileVersion = ExtractVersion(destFileName) ?? 0;
        var destPath = Path.Combine(pluginsPath, destFileName);

        ReportProgress(15, "检查现有版本...");
        var existingFiles = Directory.GetFiles(pluginsPath, "SirenSetting_Limit_Adjuster_b*.asi");
        var toBackup = existingFiles
            .Select(f => (Path: f, Ver: ExtractVersion(Path.GetFileName(f))))
            .Where(x => x.Ver.HasValue && x.Ver.Value < fileVersion)
            .ToList();

        if (toBackup.Count > 0 || File.Exists(destPath))
        {
            var msg = toBackup.Count > 0
                ? $"检测到 {toBackup.Count} 个旧版本（版本号 < b{fileVersion}）\n将全部备份后安装，是否继续？"
                : $"目标位置已存在同名文件，是否替换？\n（旧文件将移入备份文件夹）";
            var result = MessageBox.Show(msg, "确认替换", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                Log("用户取消替换");
                ReportProgress(0, "已取消");
                lblStatus.ForeColor = Color.FromArgb(140, 150, 160);
                return;
            }
        }

        ReportProgress(25, "备份旧版本...");
        var backupFolder = Path.Combine(pluginsPath, "SirenSetting_Limit_Adjuster_备份");
        if (!Directory.Exists(backupFolder))
        {
            Directory.CreateDirectory(backupFolder);
            Log($"已创建备份文件夹: {backupFolder}");
        }

        var step = toBackup.Count > 0 ? 30 / Math.Max(1, toBackup.Count + 1) : 0;
        var p = 25;
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
            ReportProgress(60, "备份完成");
            Log($"已备份并删除: {Path.GetFileName(destPath)}");
        }
        else if (toBackup.Count > 0)
        {
            ReportProgress(60, "备份完成");
        }

        ReportProgress(80, "安装中...");
        File.Copy(sourcePath, destPath, overwrite: true);
        Log($"已安装: {destPath}");

        ReportProgress(100, "成功");
        lblStatus.ForeColor = Color.FromArgb(120, 200, 140);
        MessageBox.Show($"SirenSetting Limit Adjuster 已成功安装到:\n{destPath}\n\n（从本地文件安装，适用于无法访问 GitHub 的用户）", "安装完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        Log("安装完成");
    }

    private async Task<byte[]> DownloadAsiAsync()
    {
        ReportProgress(5, "正在下载...");
        Log("正在下载 ASI 文件...");

        // 1) 先试官方 raw（带实时进度）
        try
        {
            var response = await HttpClient.GetAsync(RawAsiUrl, HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode)
            {
                var bytes = await DownloadStreamWithProgressAsync(response, 5, 35, "ASI");
                ReportProgress(35, "下载完成");
                return bytes;
            }
        }
        catch { /* 官方不可达，试备用 */ }

        // 2) 再试国内代理（带实时进度）
        foreach (var prefix in RawProxyPrefixes)
        {
            try
            {
                var proxyUrl = prefix + RawAsiUrl;
                Log($"尝试备用: {new Uri(proxyUrl).Host} ...");
                var response = await HttpClient.GetAsync(proxyUrl, HttpCompletionOption.ResponseHeadersRead);
                if (response.IsSuccessStatusCode)
                {
                    var bytes = await DownloadStreamWithProgressAsync(response, 5, 35, "ASI");
                    ReportProgress(35, "下载完成");
                    return bytes;
                }
            }
            catch { /* 该代理不可用，试下一个 */ }
        }

        // 3) 最后从仓库 zip 提取（先试官方，失败再试代理，带实时进度）
        ReportProgress(15, "从压缩包提取...");
        Log("直链与备用均失败，尝试从仓库压缩包提取...");
        byte[]? zipBytes = null;
        try
        {
            var response = await HttpClient.GetAsync(ZipArchiveUrl, HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode)
                zipBytes = await DownloadStreamWithProgressAsync(response, 15, 30, "压缩包");
        }
        catch { /* 官方 zip 不可达 */ }
        if (zipBytes == null || zipBytes.Length == 0)
        {
            foreach (var prefix in RawProxyPrefixes)
            {
                try
                {
                    var response = await HttpClient.GetAsync(prefix + ZipArchiveUrl, HttpCompletionOption.ResponseHeadersRead);
                    if (response.IsSuccessStatusCode)
                    {
                        zipBytes = await DownloadStreamWithProgressAsync(response, 15, 30, "压缩包");
                        Log($"通过 {new Uri(prefix).Host} 获取压缩包");
                        break;
                    }
                }
                catch { /* 试下一个 */ }
            }
        }
        if (zipBytes == null || zipBytes.Length == 0)
            throw new Exception("无法从 GitHub 或备用代理获取文件，请使用「从本地安装」选择已下载的 .asi 文件。");

        ReportProgress(32, "解压中...");
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

    /// <summary>
    /// 按块读取响应流并报告进度到进度条与日志（实时控制台）
    /// </summary>
    private async Task<byte[]> DownloadStreamWithProgressAsync(HttpResponseMessage response, int progressStart, int progressEnd, string logLabel)
    {
        var total = response.Content.Headers.ContentLength;
        await using var stream = await response.Content.ReadAsStreamAsync();
        using var ms = new MemoryStream(total > 0 ? (int)total : 0);
        var buffer = new byte[32 * 1024];
        long received = 0;
        int lastLoggedPct = -1;
        int lastLoggedKb = 0;
        int range = Math.Max(1, progressEnd - progressStart);

        while (true)
        {
            int read = await stream.ReadAsync(buffer);
            if (read == 0) break;
            ms.Write(buffer, 0, read);
            received += read;

            int pct = total.HasValue && total.Value > 0
                ? (int)(100 * received / total.Value)
                : 0;
            int barPct = progressStart + (int)(range * (total.HasValue && total.Value > 0 ? (double)received / total.Value : 0.5));
            ReportProgress(Math.Clamp(barPct, progressStart, progressEnd), total.HasValue ? $"下载 {received / 1024} / {total.Value / 1024} KB ({pct}%)" : $"下载 {received / 1024} KB");

            if (total.HasValue && total.Value > 0 && pct >= lastLoggedPct + 10)
            {
                lastLoggedPct = (pct / 10) * 10;
                Log($"{logLabel} 已下载: {received / 1024} KB / {total.Value / 1024} KB ({pct}%)");
            }
            else if (!total.HasValue)
            {
                int kb = (int)(received / 1024);
                if (kb >= lastLoggedKb + 200)
                {
                    lastLoggedKb = kb;
                    Log($"{logLabel} 已下载: {kb} KB");
                }
            }
        }

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
