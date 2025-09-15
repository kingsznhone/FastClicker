using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mouse_click
{
    [SupportedOSPlatform("windows")]
    public partial class Form1 : Form
    {
        private const int WM_HOTKEY = 0x312;
        private const int WM_CREATE = 0x1;
        private const int WM_DESTROY = 0x2;
        private const int Space = 0x3572;
        bool Enable = false;
        [DllImport("user32.dll", EntryPoint = "mouse_event")]
        public static extern void mouse_event(
            int dwFlags,
            int dx,
            int dy,
            int cButtons,
            int dwExtraInfo
        );
        [DllImport("user32.dll", EntryPoint = "keybd_event")]
        public static extern void keybd_event(
           byte bVk,
           byte bScan,
           int dwFlags,
           int dwExtraInfo
       );
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point p);
        Point p;
        const int MOUSEEVENTF_MOVE = 0x0001;
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;

        public Form1()
        {
            InitializeComponent();
        }

        private async void timer1_Tick(object sender, EventArgs e)
        {
            keybd_event((byte)Keys.Down, 0, 0, 0);
            await Task.Delay(14);
            keybd_event((byte)Keys.Up, 0, 0, 0);
            await Task.Delay(14);
            keybd_event((byte)Keys.Left, 0, 0, 0);
            await Task.Delay(14);
            keybd_event((byte)Keys.Right, 0, 0, 0);
            await Task.Delay(14);
            keybd_event((byte)Keys.Up, 0, 2, 0);
            await Task.Delay(14);
            keybd_event((byte)Keys.Down, 0, 2, 0);
            await Task.Delay(14);
            keybd_event((byte)Keys.Left, 0, 2, 0);
            await Task.Delay(14);
            keybd_event((byte)Keys.Right, 0, 2, 0);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);
            switch (m.Msg)
            {
                case WM_HOTKEY:
                    switch (m.WParam.ToInt32())
                    {
                        case Space:
                            timer1.Enabled = !timer1.Enabled;
                            if (timer1.Enabled)
                            {
                                pictureBox1.Image = Properties.Resources.on;
                            }
                            else
                            {
                                pictureBox1.Image = Properties.Resources.off;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
                case WM_CREATE:
                    AppHotKey.RegKey(Handle, Space, AppHotKey.KeyModifiers.Ctrl | AppHotKey.KeyModifiers.Shift, Keys.D);
                    break;
                case WM_DESTROY:
                    AppHotKey.UnRegKey(Handle, Space);
                    break;
                default:
                    break;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            AppHotKey.UnRegKey(Handle, Space);
            Environment.Exit(0);
        }
    }
}
