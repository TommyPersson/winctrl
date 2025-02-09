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
            
            var process = Process.GetProcessById(Convert.ToInt32(processId));
                
            return Task.FromResult(new ActiveWindowInfoDTO
            {
                WindowTitle = windowTitle,
                ExecutableName = process.MainModule?.ModuleName,
                ExecutableFileName = process.MainModule?.FileName,
                ProcessName = process.ProcessName,
                ProcessId = process.Id,
            });
        }

        public async Task StartProcess(string executable)
        {
            Process.Start(executable);
        }

        public async Task OpenUrl(string url)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true
            });
        }
    }
}