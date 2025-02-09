using System.CommandLine;
using System.Threading.Tasks;
using winctrl.Modules.Executables;
using winctrl.Modules.Processes;
using winctrl.Modules.SystemMedia;
using winctrl.Modules.WebApi;
using winctrl.Output;

namespace winctrl
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var commandProcessor = new CommandProcessor(new JsonOutputRenderer());

            var rootCommand = new RootCommand("Windows Controls");
            rootCommand.Add(new SystemMediaCommand(commandProcessor));
            rootCommand.Add(new ProcessesCommand(commandProcessor));
            rootCommand.Add(new ExecutablesCommand(commandProcessor));
            rootCommand.Add(new WebApiCommand());

            await rootCommand.InvokeAsync(args);
        }
    }
}
