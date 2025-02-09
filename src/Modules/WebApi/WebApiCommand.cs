using System.CommandLine;

namespace winctrl.Modules.WebApi
{
    class WebApiCommand : Command
    {
        public WebApiCommand() : base(name: "web-api", description: "Starts the program as a web API")
        {
            this.SetHandler(
                () => { WebApiStartup.WebApiStartup.Run(); });
        }
    }
}