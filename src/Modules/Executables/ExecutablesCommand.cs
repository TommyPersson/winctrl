using System.CommandLine;

namespace winctrl.Modules.Executables
{
    public class ExecutablesCommand : Command
    {
        public ExecutablesCommand(CommandProcessor processor) : base(
            name: "executables",
            description: "Commands related to executables"
        )
        {
            Add(new IconsCommand(processor));
            Add(new IconDataCommand(processor));
        }
    }

    public class IconsCommand : Command
    {
        private readonly ExecutablesService _service = new ExecutablesService();

        public IconsCommand(CommandProcessor processor) : base(name: "icons")
        {
            var filePathArgument = new Argument<string>(
                name: "file-path",
                description: "The path to the executable file"
            );
            Add(filePathArgument);
            this.SetHandler(
                (filePath) => processor.Process(() => _service.GetIconsForFile(filePath)),
                filePathArgument
            );
        }
    }

    public class IconDataCommand : Command
    {
        private readonly ExecutablesService _service = new ExecutablesService();

        public IconDataCommand(CommandProcessor processor) : base(name: "icon-data")
        {
            var filePathArgument = new Argument<string>(
                name: "file-path",
                description: "The path to the executable file"
            );
            var iconIndexOption = new Option<int>("icon-index", getDefaultValue: () => 0);
            var iconSizeOption = new Option<string>("icon-size", getDefaultValue: () => "32x32");
            
            Add(filePathArgument);
            Add(iconIndexOption);
            Add(iconSizeOption);
            this.SetHandler(
                (filePath, index, size) => processor.Process(() => _service.GetIconData(filePath, index, size)),
                filePathArgument, iconIndexOption, iconSizeOption
            );
        }
    }
}