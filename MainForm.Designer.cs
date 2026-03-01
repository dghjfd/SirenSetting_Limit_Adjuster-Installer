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
        // MainForm
        //
        AutoScaleDimensions = new SizeF(7F, 17F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.FromArgb(32, 36, 44);
        ClientSize = new Size(580, 385);
        Controls.Add(lblCopyright);
        Controls.Add(txtLog);
        Controls.Add(lblLog);
        Controls.Add(progressBar);
        Controls.Add(lblStatus);
        Controls.Add(btnCheckUpdate);
        Controls.Add(btnInstallLocal);
        Controls.Add(btnInstall);
        Controls.Add(btnScan);
        Controls.Add(btnBrowse);
        Controls.Add(txtPath);
        Controls.Add(lblPath);
        Controls.Add(lblTitle);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        MinimumSize = new Size(400, 400);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "TEA Siren Installer";
        Load += MainForm_Load;
        ResumeLayout(false);
        PerformLayout();
    }

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

