using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace lab6
{
    public partial class Form1 : Form
    {

        delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);
        string GetWindowText(IntPtr hWnd)
        {
            int len = GetWindowTextLength(hWnd) + 1;
            StringBuilder sb = new StringBuilder(len);
            len = GetWindowText(hWnd, sb, len);
            return sb.ToString(0, len);
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool IsWindowVisible(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowText(IntPtr hWnd, string text);

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool GetCursorPos(out POINT lpPoint);
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;
        }
        public static POINT GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            return lpPoint;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int Width, int Height, bool Repaint);

        



        public Form1()
        {
            InitializeComponent();
        }

        System.Timers.Timer timer1;

        public delegate void InvokeDelegate();
        public string selitem;
        WinInf winInf = new WinInf();
        MouseFlags mouseFlags = new MouseFlags();

        private void filler()
        {
            listBox1.Items.Clear();
            EnumWindows((hWnd, lParam) => {
                if (IsWindowVisible(hWnd) && GetWindowTextLength(hWnd) != 0)
                {
                    listBox1.Items.Add(GetWindowText(hWnd));
                }
                return true;
            }, IntPtr.Zero);
        }

        private void EventFill(object sender, EventArgs e)
        {
            listBox1.BeginInvoke(new InvokeDelegate(filler));
        }

        private void EventText(object sender, EventArgs e)
        {
            SetWindowText(winInf.hwnd, textBox1.Text);
            selitem = GetWindowText(winInf.hwnd);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Size resolution = Screen.PrimaryScreen.Bounds.Size;
            pictureBox1.Width = resolution.Width / 10;
            pictureBox1.Height = resolution.Height / 10;
            hScrollBar1.Maximum= resolution.Width;
            vScrollBar1.Maximum= resolution.Height;
            hScrollBar2.Maximum= 3;


            timer1 = new System.Timers.Timer();

            listBox1.Items.Clear();

            timer1.Interval= 1000;

            timer1.Elapsed += EventFill;
            timer1.Elapsed += EventText;

            timer1.Start();
            textBox2.Text = hScrollBar1.Value.ToString();
            textBox3.Text = vScrollBar1.Value.ToString();
            

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                
                winInf.hwnd = FindWindow(null, listBox1.SelectedItem.ToString());

                RECT rc = new RECT();
                GetWindowRect(winInf.hwnd, ref rc);

                winInf.width = rc.Right - rc.Left;
                winInf.height = rc.Bottom - rc.Top;
                winInf.x = rc.Left;
                winInf.y = rc.Top;

                textBox1.Text = GetWindowText(winInf.hwnd);
                ScreenCheker();
            }
            
        }

        private void ScreenCheker()
        {
            Size resolution = Screen.PrimaryScreen.Bounds.Size;
            Console.WriteLine("Width: {0}, Height: {1}", resolution.Width, resolution.Height);

        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            textBox2.Text = hScrollBar1.Value.ToString();
            MoveWindow(winInf.hwnd, hScrollBar1.Value, winInf.y, winInf.width, winInf.height, true);
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            textBox3.Text = vScrollBar1.Value.ToString();
            MoveWindow(winInf.hwnd, winInf.x, vScrollBar1.Value, winInf.width, winInf.height, true);
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if(e.Button== MouseButtons.Left)
            {
                POINT cursorpos = GetCursorPosition();
                Point df = pictureBox1.PointToClient(MousePosition);
                int x = df.X*10;
                int y = df.Y*10;
                
                MoveWindow(winInf.hwnd, x, y, winInf.width, winInf.height, true);
            }
        }

        private void hScrollBar2_ValueChanged(object sender, EventArgs e)
        {
            int height = winInf.height * hScrollBar2.Value;
            int width = winInf.width * hScrollBar2.Value;
            Console.WriteLine("ScrollBarValue" + hScrollBar2.Value + "height= " + height + "width= " + width);
            MoveWindow(winInf.hwnd, winInf.x, winInf.y, width, height, true);
        }
    }
}
