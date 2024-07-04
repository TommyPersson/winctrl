using System.CommandLine;

namespace winctrl.Modules.Processes
{
    public class ProcessesCommand : Command
    {
        public ProcessesCommand(CommandProcessor commandProcessor) : base("processes", "Process utilities")
        {
            var service = new ProcessesService();
            
            var activeWindowCommand = new Command("active-window", "Commands related to the currently active window");
            var activeWindowInfoCommand = new Command("info", "Prints information about the current window");
            
            activeWindowInfoCommand.SetHandler(() => commandProcessor.Process(() => service.GetActiveWindowInfoAsync()));
            activeWindowCommand.Add(activeWindowInfoCommand);
            
            Add(activeWindowCommand);
        }
    }
}