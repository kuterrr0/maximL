using System.Drawing;
using System.Runtime.InteropServices;

namespace lab7
{
    public partial class Form1 : Form
    {
        //Типы данных для работы
        internal enum HookType : int
        {
            WH_JOURNALRECORD = 0,
            WH_JOURNALPLAYBACK = 1,
            WH_KEYBOARD = 2,
            WH_GETMESSAGE = 3,
            WH_CALLWNDPROC = 4,
            WH_CBT = 5,
            WH_SYSMSGFILTER = 6,
            WH_MOUSE = 7,
            WH_HARDWARE = 8,
            WH_DEBUG = 9,
            WH_SHELL = 10,
            WH_FOREGROUNDIDLE = 11,
            WH_CALLWNDPROCRET = 12,
            WH_KEYBOARD_LL = 13,
            WH_MOUSE_LL = 14
        }
        private struct POINT
        {
            public int x;
            public int y;
        }
        private struct MSLLHOOKSTRUCT
        {
            public POINT pt;
            public uint mouseData;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        private enum MouseMessages
        {
            WM_LBUTTONDOWN = 0x0201,
            WM_LBUTTONUP = 0x0202,
            WM_MOUSEMOVE = 0x0200,
            WM_MOUSEWHEEL = 0x020A,
            WM_RBUTTONDOWN = 0x0204,
            WM_RBUTTONUP = 0x0205
        }

        //

        private static string Dest = "";
        private static Queue<string> filing_queue = new Queue<string>();
        private static bool be_filing = false;
        private Thread filing = new Thread(() => { });

        private delegate int HookProc(int nCode, IntPtr wParam, IntPtr lParam);
        static HookProc KeyboardHandle = new HookProc(KeyboardHookProc);
        static HookProc MouseHandle = new HookProc(MouseHookProc);

        private static IntPtr MHook = IntPtr.Zero;
        private static IntPtr KHook = IntPtr.Zero;
        static HookProc hookProc = new HookProc(KeyboardHookProc);

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll")]
        static extern int CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);

        public const int WH_KEYBOARD_LL = 13;

        public Form1()
        {
            InitializeComponent();
            Dest = Path.Combine(Directory.GetCurrentDirectory(), "logger.txt");
        }
        private static IntPtr InstallHook(int ID, HookProc func)
        {
            IntPtr hInstance = GetModuleHandle("User32");
            return SetWindowsHookEx(ID, func, hInstance, 0);
        }

        public static int KeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)0x0100)
            {
                int KeyCode = Marshal.ReadInt32(lParam);
                string outputting = "[ " + DateTime.Now.ToString() + " ] Keyboard: " + ((Keys)KeyCode).ToString() + "\n";
                lock (filing_queue)
                {
                    filing_queue.Enqueue(outputting);
                }
            }
            return CallNextHookEx(KHook, nCode, wParam, lParam);
        }

        public static int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && MouseMessages.WM_MOUSEMOVE != (MouseMessages)wParam)
            {
                MSLLHOOKSTRUCT MouseStruct = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
                string outputting = "[ " + DateTime.Now.ToString() + " ] Mouse: Pos: [X: " + MouseStruct.pt.x.ToString() + ", Y: " + MouseStruct.pt.y.ToString() + "] Action: " + ((MouseMessages)wParam).ToString() + "\n";
                lock (filing_queue)
                {
                    filing_queue.Enqueue(outputting);
                }
            }
            return CallNextHookEx(MHook, nCode, wParam, lParam);
        }

        private void start_hooks()
        {
            filing = new Thread(() =>
            {
                string element = "";
                while (be_filing)
                {
                    lock (filing_queue)
                    {
                        if (filing_queue.Count > 0)
                        {
                            element = filing_queue.Dequeue();
                            File.AppendAllText(Dest, element);
                        }
                    }
                }
            });
            be_filing = true;
            filing.Start();

            KHook = InstallHook((int)HookType.WH_KEYBOARD_LL, KeyboardHandle);
            MHook = InstallHook((int)HookType.WH_MOUSE_LL, MouseHandle);
        }


        private void end_hooks()
        {
            if (MHook != IntPtr.Zero)
            {
                be_filing = false;
                UnhookWindowsHookEx(KHook);
                UnhookWindowsHookEx(MHook);
                KHook = IntPtr.Zero;
                MHook = IntPtr.Zero;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.BackColor == Color.LightGreen)
            {
                start_hooks();
                button1.Text = "Стоп";
                button1.BackColor = Color.IndianRed;
            }
            else
            {
                end_hooks();
                button1.Text = "Старт";
                button1.BackColor = Color.LightGreen;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            end_hooks();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (File.Exists(Dest) && MHook == IntPtr.Zero)
            {
                File.Delete(Dest);///.Dispose();
            }
            else
            {
                MessageBox.Show("Нет логов или они заняты +.-", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (File.Exists(Dest) && MHook == IntPtr.Zero)
            {
                System.Diagnostics.Process.Start("notepad",Dest);
            }
            else
            {
                MessageBox.Show("Нет логов или они заняты +.-", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
