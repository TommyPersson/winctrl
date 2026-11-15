using System;
using System.Runtime.InteropServices;
using System.Text;

namespace winctrl.Utils
{
    class Win32
    {
        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern UInt32 GetWindowThreadProcessId(IntPtr hWnd, out UInt32 lpdwProcessId);

        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetForegroundWindow();
        
        public static UInt32 GetWindowThreadProcessId(IntPtr  hWnd)
        {
            UInt32 processId;
            GetWindowThreadProcessId(hWnd, out processId);
            return processId;
        }
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowTextLength(IntPtr hWnd);

        public static string GetWindowTitle(IntPtr hWnd)
        {
            var length = GetWindowTextLength(hWnd) + 1;
            var titleBuilder = new StringBuilder(length);
            GetWindowText(hWnd, titleBuilder, length);
            return titleBuilder.ToString();
        }
        
        [DllImport("shell32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint ExtractIconEx(string lpszFile, int nIconIndex, [Out] IntPtr[] phiconLarge, [Out] IntPtr[] phiconSmall, [In] uint nIcons);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int DestroyIcon(IntPtr hIcon);

        public const int SW_SHOW = 5;
        public const int SW_RESTORE = 9;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool BringWindowToTop(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsIconic(IntPtr hWnd);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool AttachThreadInput(uint idAttach, uint idAttachTo, [MarshalAs(UnmanagedType.Bool)] bool fAttach);

        [DllImport("kernel32.dll")]
        public static extern uint GetCurrentThreadId();

        public const byte VK_MENU = 0x12;
        public const uint KEYEVENTF_EXTENDEDKEY = 0x0001;
        public const uint KEYEVENTF_KEYUP = 0x0002;

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        /// <summary>
        /// Brings a window to the front, working around the foreground lock that otherwise makes
        /// SetForegroundWindow just flash the taskbar button when called from a background process.
        /// </summary>
        public static bool ForceForegroundWindow(IntPtr hWnd)
        {
            ShowWindow(hWnd, IsIconic(hWnd) ? SW_RESTORE : SW_SHOW);

            if (GetForegroundWindow() == hWnd || SetForegroundWindow(hWnd))
            {
                return true;
            }

            // Share input state with the foreground window's thread, which lets this thread change the foreground.
            var foregroundThreadId = GetWindowThreadProcessId(GetForegroundWindow(), out _);
            var currentThreadId = GetCurrentThreadId();
            var attached = foregroundThreadId != currentThreadId
                           && AttachThreadInput(currentThreadId, foregroundThreadId, true);
            try
            {
                BringWindowToTop(hWnd);
                SetForegroundWindow(hWnd);
            }
            finally
            {
                if (attached)
                {
                    AttachThreadInput(currentThreadId, foregroundThreadId, false);
                }
            }

            if (GetForegroundWindow() == hWnd)
            {
                return true;
            }

            // Last resort: a synthetic Alt press makes this process the one that received the last input event,
            // which lifts the lock.
            keybd_event(VK_MENU, 0, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero);
            keybd_event(VK_MENU, 0, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, UIntPtr.Zero);
            SetForegroundWindow(hWnd);

            return GetForegroundWindow() == hWnd;
        }
    }
}