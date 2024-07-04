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
    }
}