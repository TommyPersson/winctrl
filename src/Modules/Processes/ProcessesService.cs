using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using winctrl.Modules.Processes.Contracts;
using winctrl.Utils;

namespace winctrl.Modules.Processes
{
    public class ProcessesService
    {
        public Task<ActiveWindowInfoDTO> GetActiveWindowInfoAsync()
        {
            var activeWindowHandle = Win32.GetForegroundWindow();
            var windowTitle = Win32.GetWindowTitle(activeWindowHandle);
            var processId = Win32.GetWindowThreadProcessId(activeWindowHandle);
            
            using (var process = Process.GetProcessById(Convert.ToInt32(processId)))
            {
                // MainModule enumerates the target's modules on each access; read it once.
                var mainModule = process.MainModule;
                return Task.FromResult(new ActiveWindowInfoDTO
                {
                    WindowTitle = windowTitle,
                    ExecutableName = mainModule?.ModuleName,
                    ExecutableFileName = mainModule?.FileName,
                    ProcessName = process.ProcessName,
                    ProcessId = process.Id,
                });
            }
        }

        public async Task StartProcess(string executable)
        {
            Process.Start(executable)?.Dispose();
        }

        public async Task OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true
            })?.Dispose();
        }

        /// <summary>
        /// Brings the main window of a process to the front, e.g. after opening a link handled by an app that is
        /// already running, which Windows otherwise only flashes in the taskbar.
        /// </summary>
        /// <returns><c>null</c> if no such process has a window, otherwise whether it was brought to the front.</returns>
        public Task<bool?> FocusWindow(string processName)
        {
            var name = processName.Trim();
            if (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            {
                name = name.Substring(0, name.Length - 4);
            }

            var processes = Process.GetProcessesByName(name);
            try
            {
                var window = processes
                    .Select(it => it.MainWindowHandle)
                    .FirstOrDefault(it => it != IntPtr.Zero);

                if (window == IntPtr.Zero)
                {
                    return Task.FromResult<bool?>(null);
                }

                return Task.FromResult<bool?>(Win32.ForceForegroundWindow(window));
            }
            finally
            {
                foreach (var process in processes)
                {
                    process.Dispose();
                }
            }
        }
    }
}