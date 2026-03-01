using System.Drawing;

namespace TEA_siren;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _formIcon?.Dispose();
            components?.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        tabControl = new TabControl();
        tabInstall = new TabPage();
        tabOptions = new TabPage();
        tabSponsors = new TabPage();
        tabChangelog = new TabPage();
        lblTitle = new Label();
        lblPath = new Label();
        txtPath = new TextBox();
        btnBrowse = new Button();
        btnScan = new Button();
        btnInstall = new Button();
        btnInstallLocal = new Button();
        btnCheckUpdate = new Button();
        lblStatus = new Label();
        progressBar = new ProgressBar();
        lblLog = new Label();
        txtLog = new TextBox();
        lblCopyright = new Label();
        lblOptionLanguage = new Label();
        comboLanguage = new ComboBox();
        chkCheckUpdateStartup = new CheckBox();
        lblSponsorsTitle = new Label();
        txtSponsors = new TextBox();
        lblChangelogTitle = new Label();
        btnChangelogLoad = new Button();
        txtChangelog = new TextBox();
        tabControl.SuspendLayout();
        tabInstall.SuspendLayout();
        tabOptions.SuspendLayout();
        tabSponsors.SuspendLayout();
        tabChangelog.SuspendLayout();
        SuspendLayout();
        //
        // lblTitle
        //
        lblTitle.AutoSize = true;
        lblTitle.Font = new Font("Microsoft YaHei UI", 12F, FontStyle.Bold);
        lblTitle.ForeColor = Color.FromArgb(230, 220, 200);
        lblTitle.Location = new Point(20, 20);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(280, 22);
        lblTitle.TabIndex = 0;
        lblTitle.Text = "SirenSetting Limit Adjuster 安装器";
        //
        // lblPath
        //
        lblPath.AutoSize = true;
        lblPath.ForeColor = Color.FromArgb(190, 195, 200);
        lblPath.Location = new Point(20, 60);
        lblPath.Name = "lblPath";
        lblPath.Size = new Size(92, 17);
        lblPath.TabIndex = 1;
        lblPath.Text = "Plugins 目录:";
        //
        // txtPath
        //
        txtPath.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        txtPath.BackColor = Color.FromArgb(38, 42, 52);
        txtPath.BorderStyle = BorderStyle.FixedSingle;
        txtPath.ForeColor = Color.FromArgb(220, 222, 228);
        txtPath.Location = new Point(20, 80);
        txtPath.Name = "txtPath";
        txtPath.ReadOnly = true;
        txtPath.Size = new Size(440, 23);
        txtPath.TabIndex = 2;
        txtPath.PlaceholderText = "选择或扫描目录...";
        //
        // btnBrowse
        //
        btnBrowse.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        btnBrowse.BackColor = Color.FromArgb(72, 88, 112);
        btnBrowse.FlatStyle = FlatStyle.Flat;
        btnBrowse.FlatAppearance.BorderColor = Color.FromArgb(100, 120, 150);
        btnBrowse.FlatAppearance.BorderSize = 1;
        btnBrowse.ForeColor = Color.FromArgb(235, 238, 242);
        btnBrowse.Location = new Point(470, 78);
        btnBrowse.Name = "btnBrowse";
        btnBrowse.Size = new Size(90, 28);
        btnBrowse.TabIndex = 3;
        btnBrowse.Text = "选择目录";
        btnBrowse.UseVisualStyleBackColor = false;
        btnBrowse.Click += BtnBrowse_Click;
        //
        // btnScan
        //
        btnScan.BackColor = Color.FromArgb(50, 56, 68);
        btnScan.FlatStyle = FlatStyle.Flat;
        btnScan.FlatAppearance.BorderColor = Color.FromArgb(90, 100, 120);
        btnScan.FlatAppearance.BorderSize = 1;
        btnScan.ForeColor = Color.FromArgb(210, 215, 220);
        btnScan.Location = new Point(20, 120);
        btnScan.Name = "btnScan";
        btnScan.Size = new Size(120, 32);
        btnScan.TabIndex = 4;
        btnScan.Text = "自动扫描";
        btnScan.UseVisualStyleBackColor = false;
        btnScan.Click += BtnScan_Click;
        //
        // btnInstall
        //
        btnInstall.BackColor = Color.FromArgb(88, 110, 140);
        btnInstall.FlatStyle = FlatStyle.Flat;
        btnInstall.FlatAppearance.BorderColor = Color.FromArgb(110, 135, 170);
        btnInstall.FlatAppearance.BorderSize = 1;
        btnInstall.ForeColor = Color.FromArgb(248, 248, 250);
        btnInstall.Location = new Point(150, 120);
        btnInstall.Name = "btnInstall";
        btnInstall.Size = new Size(120, 32);
        btnInstall.TabIndex = 5;
        btnInstall.Text = "安装插件";
        btnInstall.UseVisualStyleBackColor = false;
        btnInstall.Click += BtnInstall_Click;
        //
        // btnInstallLocal
        //
        btnInstallLocal.BackColor = Color.FromArgb(60, 75, 95);
        btnInstallLocal.FlatStyle = FlatStyle.Flat;
        btnInstallLocal.FlatAppearance.BorderColor = Color.FromArgb(90, 110, 135);
        btnInstallLocal.FlatAppearance.BorderSize = 1;
        btnInstallLocal.ForeColor = Color.FromArgb(235, 238, 242);
        btnInstallLocal.Location = new Point(280, 120);
        btnInstallLocal.Name = "btnInstallLocal";
        btnInstallLocal.Size = new Size(120, 32);
        btnInstallLocal.TabIndex = 11;
        btnInstallLocal.Text = "从本地安装";
        btnInstallLocal.UseVisualStyleBackColor = false;
        btnInstallLocal.Click += BtnInstallLocal_Click;
        //
        // btnCheckUpdate
        //
        btnCheckUpdate.BackColor = Color.FromArgb(55, 65, 80);
        btnCheckUpdate.FlatStyle = FlatStyle.Flat;
        btnCheckUpdate.FlatAppearance.BorderColor = Color.FromArgb(80, 95, 115);
        btnCheckUpdate.FlatAppearance.BorderSize = 1;
        btnCheckUpdate.ForeColor = Color.FromArgb(210, 215, 220);
        btnCheckUpdate.Location = new Point(410, 120);
        btnCheckUpdate.Name = "btnCheckUpdate";
        btnCheckUpdate.Size = new Size(85, 32);
        btnCheckUpdate.TabIndex = 12;
        btnCheckUpdate.Text = "检查更新";
        btnCheckUpdate.UseVisualStyleBackColor = false;
        btnCheckUpdate.Click += BtnCheckUpdate_Click;
        //
        // lblStatus
        //
        lblStatus.AutoSize = true;
        lblStatus.ForeColor = Color.FromArgb(170, 178, 188);
        lblStatus.Location = new Point(502, 128);
        lblStatus.Name = "lblStatus";
        lblStatus.Size = new Size(32, 17);
        lblStatus.TabIndex = 6;
        lblStatus.Text = "就绪";
        //
        // progressBar
        //
        progressBar.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        progressBar.Location = new Point(20, 160);
        progressBar.Name = "progressBar";
        progressBar.Size = new Size(540, 22);
        progressBar.Style = ProgressBarStyle.Blocks;
        progressBar.Minimum = 0;
        progressBar.Maximum = 100;
        progressBar.Value = 0;
        progressBar.TabIndex = 7;
        //
        // lblLog
        //
        lblLog.AutoSize = true;
        lblLog.ForeColor = Color.FromArgb(180, 188, 198);
        lblLog.Location = new Point(20, 198);
        lblLog.Name = "lblLog";
        lblLog.Size = new Size(44, 17);
        lblLog.TabIndex = 8;
        lblLog.Text = "日志:";
        //
        // txtLog
        //
        txtLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtLog.BackColor = Color.FromArgb(28, 32, 40);
        txtLog.BorderStyle = BorderStyle.FixedSingle;
        txtLog.Font = new Font("Consolas", 9F);
        txtLog.ForeColor = Color.FromArgb(180, 195, 185);
        txtLog.Location = new Point(20, 218);
        txtLog.Multiline = true;
        txtLog.Name = "txtLog";
        txtLog.ReadOnly = true;
        txtLog.ScrollBars = ScrollBars.Vertical;
        txtLog.Size = new Size(540, 142);
        txtLog.TabIndex = 9;
        txtLog.WordWrap = false;
        //
        // lblCopyright
        //
        lblCopyright.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        lblCopyright.AutoSize = true;
        lblCopyright.ForeColor = Color.FromArgb(120, 130, 140);
        lblCopyright.Location = new Point(380, 368);
        lblCopyright.Name = "lblCopyright";
        lblCopyright.Size = new Size(140, 17);
        lblCopyright.TabIndex = 10;
        lblCopyright.Text = "© TEARLESSVVOID";
        //
        // tabControl
        //
        tabControl.Controls.Add(tabInstall);
        tabControl.Controls.Add(tabOptions);
        tabControl.Controls.Add(tabSponsors);
        tabControl.Controls.Add(tabChangelog);
        tabControl.Location = new Point(12, 12);
        tabControl.Name = "tabControl";
        tabControl.SelectedIndex = 0;
        tabControl.Size = new Size(556, 338);
        tabControl.TabIndex = 20;
        //
        // tabInstall
        //
        tabInstall.BackColor = Color.FromArgb(32, 36, 44);
        tabInstall.Controls.Add(lblTitle);
        tabInstall.Controls.Add(lblPath);
        tabInstall.Controls.Add(txtPath);
        tabInstall.Controls.Add(btnBrowse);
        tabInstall.Controls.Add(btnScan);
        tabInstall.Controls.Add(btnInstall);
        tabInstall.Controls.Add(btnInstallLocal);
        tabInstall.Controls.Add(btnCheckUpdate);
        tabInstall.Controls.Add(lblStatus);
        tabInstall.Controls.Add(progressBar);
        tabInstall.Controls.Add(lblLog);
        tabInstall.Controls.Add(txtLog);
        tabInstall.Location = new Point(4, 26);
        tabInstall.Name = "tabInstall";
        tabInstall.Padding = new Padding(3);
        tabInstall.Size = new Size(548, 308);
        tabInstall.TabIndex = 0;
        tabInstall.Text = "Install";
        tabInstall.UseVisualStyleBackColor = true;
        //
        // tabOptions
        //
        tabOptions.BackColor = Color.FromArgb(32, 36, 44);
        tabOptions.Controls.Add(lblOptionLanguage);
        tabOptions.Controls.Add(comboLanguage);
        tabOptions.Controls.Add(chkCheckUpdateStartup);
        tabOptions.Location = new Point(4, 26);
        tabOptions.Name = "tabOptions";
        tabOptions.Size = new Size(548, 308);
        tabOptions.TabIndex = 1;
        tabOptions.Text = "Options";
        tabOptions.UseVisualStyleBackColor = true;
        //
        // lblOptionLanguage
        //
        lblOptionLanguage.AutoSize = true;
        lblOptionLanguage.ForeColor = Color.FromArgb(190, 195, 200);
        lblOptionLanguage.Location = new Point(20, 24);
        lblOptionLanguage.Name = "lblOptionLanguage";
        lblOptionLanguage.Size = new Size(60, 17);
        lblOptionLanguage.TabIndex = 0;
        lblOptionLanguage.Text = "Language:";
        //
        // comboLanguage
        //
        comboLanguage.BackColor = Color.FromArgb(38, 42, 52);
        comboLanguage.ForeColor = Color.FromArgb(220, 222, 228);
        comboLanguage.FormattingEnabled = true;
        comboLanguage.Items.AddRange(new object[] { "English", "中文" });
        comboLanguage.Location = new Point(20, 48);
        comboLanguage.Name = "comboLanguage";
        comboLanguage.Size = new Size(180, 25);
        comboLanguage.TabIndex = 1;
        comboLanguage.SelectedIndexChanged += ComboLanguage_SelectedIndexChanged;
        //
        // chkCheckUpdateStartup
        //
        chkCheckUpdateStartup.AutoSize = true;
        chkCheckUpdateStartup.ForeColor = Color.FromArgb(190, 195, 200);
        chkCheckUpdateStartup.Location = new Point(20, 88);
        chkCheckUpdateStartup.Name = "chkCheckUpdateStartup";
        chkCheckUpdateStartup.Size = new Size(200, 21);
        chkCheckUpdateStartup.TabIndex = 2;
        chkCheckUpdateStartup.Text = "Check for update on startup";
        chkCheckUpdateStartup.UseVisualStyleBackColor = true;
        //
        // tabSponsors
        //
        tabSponsors.BackColor = Color.FromArgb(32, 36, 44);
        tabSponsors.Controls.Add(lblSponsorsTitle);
        tabSponsors.Controls.Add(txtSponsors);
        tabSponsors.Location = new Point(4, 26);
        tabSponsors.Name = "tabSponsors";
        tabSponsors.Size = new Size(548, 308);
        tabSponsors.TabIndex = 2;
        tabSponsors.Text = "Sponsors";
        tabSponsors.UseVisualStyleBackColor = true;
        //
        // lblSponsorsTitle
        //
        lblSponsorsTitle.AutoSize = true;
        lblSponsorsTitle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
        lblSponsorsTitle.ForeColor = Color.FromArgb(230, 220, 200);
        lblSponsorsTitle.Location = new Point(20, 16);
        lblSponsorsTitle.Name = "lblSponsorsTitle";
        lblSponsorsTitle.Size = new Size(140, 19);
        lblSponsorsTitle.TabIndex = 0;
        lblSponsorsTitle.Text = "Sponsors & Support";
        //
        // txtSponsors
        //
        txtSponsors.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtSponsors.BackColor = Color.FromArgb(38, 42, 52);
        txtSponsors.BorderStyle = BorderStyle.FixedSingle;
        txtSponsors.ForeColor = Color.FromArgb(220, 222, 228);
        txtSponsors.Location = new Point(20, 44);
        txtSponsors.Multiline = true;
        txtSponsors.Name = "txtSponsors";
        txtSponsors.ReadOnly = true;
        txtSponsors.ScrollBars = ScrollBars.Vertical;
        txtSponsors.Size = new Size(508, 248);
        txtSponsors.TabIndex = 1;
        txtSponsors.Text = "Thanks to everyone who supports this project.";
        //
        // tabChangelog
        //
        tabChangelog.BackColor = Color.FromArgb(32, 36, 44);
        tabChangelog.Controls.Add(lblChangelogTitle);
        tabChangelog.Controls.Add(btnChangelogLoad);
        tabChangelog.Controls.Add(txtChangelog);
        tabChangelog.Location = new Point(4, 26);
        tabChangelog.Name = "tabChangelog";
        tabChangelog.Size = new Size(548, 308);
        tabChangelog.TabIndex = 3;
        tabChangelog.Text = "Changelog";
        tabChangelog.UseVisualStyleBackColor = true;
        //
        // lblChangelogTitle
        //
        lblChangelogTitle.AutoSize = true;
        lblChangelogTitle.Font = new Font("Microsoft YaHei UI", 10F, FontStyle.Bold);
        lblChangelogTitle.ForeColor = Color.FromArgb(230, 220, 200);
        lblChangelogTitle.Location = new Point(20, 16);
        lblChangelogTitle.Name = "lblChangelogTitle";
        lblChangelogTitle.Size = new Size(100, 19);
        lblChangelogTitle.TabIndex = 0;
        lblChangelogTitle.Text = "Update history";
        //
        // btnChangelogLoad
        //
        btnChangelogLoad.BackColor = Color.FromArgb(72, 88, 112);
        btnChangelogLoad.FlatStyle = FlatStyle.Flat;
        btnChangelogLoad.ForeColor = Color.FromArgb(235, 238, 242);
        btnChangelogLoad.Location = new Point(20, 44);
        btnChangelogLoad.Name = "btnChangelogLoad";
        btnChangelogLoad.Size = new Size(140, 28);
        btnChangelogLoad.TabIndex = 1;
        btnChangelogLoad.Text = "Load from GitHub";
        btnChangelogLoad.UseVisualStyleBackColor = false;
        btnChangelogLoad.Click += BtnChangelogLoad_Click;
        //
        // txtChangelog
        //
        txtChangelog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        txtChangelog.BackColor = Color.FromArgb(28, 32, 40);
        txtChangelog.BorderStyle = BorderStyle.FixedSingle;
        txtChangelog.Font = new Font("Consolas", 9F);
        txtChangelog.ForeColor = Color.FromArgb(180, 195, 185);
        txtChangelog.Location = new Point(20, 80);
        txtChangelog.Multiline = true;
        txtChangelog.Name = "txtChangelog";
        txtChangelog.ReadOnly = true;
        txtChangelog.ScrollBars = ScrollBars.Vertical;
        txtChangelog.Size = new Size(508, 212);
        txtChangelog.TabIndex = 2;
        txtChangelog.WordWrap = true;
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(32, 36, 44);
        ClientSize = new Size(580, 420);
        Controls.Add(tabControl);
        Controls.Add(lblCopyright);
        lblCopyright.Location = new Point(420, 358);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimumSize = new Size(400, 420);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "TEA Siren Installer";
        Load += MainForm_Load;
        tabControl.ResumeLayout(false);
        tabInstall.ResumeLayout(false);
        tabInstall.PerformLayout();
        tabOptions.ResumeLayout(false);
        tabOptions.PerformLayout();
        tabSponsors.ResumeLayout(false);
        tabSponsors.PerformLayout();
        tabChangelog.ResumeLayout(false);
        tabChangelog.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private TabControl tabControl;
    private TabPage tabInstall;
    private TabPage tabOptions;
    private TabPage tabSponsors;
    private TabPage tabChangelog;
    private Label lblOptionLanguage;
    private ComboBox comboLanguage;
    private CheckBox chkCheckUpdateStartup;
    private Label lblSponsorsTitle;
    private TextBox txtSponsors;
    private Label lblChangelogTitle;
    private Button btnChangelogLoad;
    private TextBox txtChangelog;
    private Label lblTitle;
    private Label lblPath;
    private TextBox txtPath;
    private Button btnBrowse;
    private Button btnScan;
    private Button btnInstall;
    private Button btnInstallLocal;
    private Button btnCheckUpdate;
    private Label lblStatus;
    private ProgressBar progressBar;
    private Label lblLog;
    private TextBox txtLog;
    private Label lblCopyright;
}

