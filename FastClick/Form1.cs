using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mouse_click
{
    public partial class Form1 : Form
    {
        private const int WM_HOTKEY = 0x312; 
        private const int WM_CREATE = 0x1;   
        private const int WM_DESTROY = 0x2;  
        private const int Space = 0x3572; 
        bool isenable = false;
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetCursorPos(out p);
            label1.Text = p.X.ToString();
            label2.Text = p.Y.ToString();
            int dx = Convert.ToInt32(p.X);
            int dy = Convert.ToInt32(p.Y);
            mouse_event(MOUSEEVENTF_LEFTDOWN, dx, dy, 0, 0);
            keybd_event((byte)Keys.D1, 0, 0, 0);
            Thread.Sleep(10);
            mouse_event(MOUSEEVENTF_LEFTUP, dx, dy, 0, 0);
            keybd_event((byte)Keys.D1, 0, 2, 0);

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
                            if (isenable)
                            {
                                timer1.Enabled = false;
                                isenable = false;
                            }
                            else
                            {
                                timer1.Enabled = true;
                                isenable = true;
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
            Environment.Exit(0);
        }
    }
}
