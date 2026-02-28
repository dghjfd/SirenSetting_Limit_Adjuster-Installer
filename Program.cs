// 入口仅启动主窗口，无后台、无驻留。全部逻辑见 MainForm。

using System;
using System.Windows.Forms;

namespace TEA_siren;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}
