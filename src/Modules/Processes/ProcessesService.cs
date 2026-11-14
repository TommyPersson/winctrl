using System;
using System.Diagnostics;
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
    }
}