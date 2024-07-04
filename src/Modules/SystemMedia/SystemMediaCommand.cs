using System.CommandLine;

namespace winctrl.Modules.SystemMedia
{
    public class SystemMediaCommand : Command
    {
        public SystemMediaCommand(CommandProcessor processor) : base("system-media", "Windows System Media Controls")
        {
            var mediaControls = new SystemMediaService();
            
            var pauseCommand = new Command("pause");
            pauseCommand.SetHandler(() => processor.Process(() => mediaControls.PauseAsync()));
        
            var playCommand = new Command("play");
            playCommand.SetHandler(() => processor.Process(() => mediaControls.PlayAsync()));
            
            var pauseOrPlayCommand = new Command("pause-or-play");
            pauseOrPlayCommand.SetHandler(() => processor.Process(() => mediaControls.PauseOrResumeAsync()));
            
            var skipNextCommand = new Command("skip-next");
            skipNextCommand.SetHandler(() => processor.Process(() => mediaControls.SkipNextAsync()));
            
            var skipPreviousCommand = new Command("skip-previous");
            skipPreviousCommand.SetHandler(() => processor.Process(() => mediaControls.SkipPreviousAsync()));
            
            var statusCommand = new Command("status");
            statusCommand.SetHandler(() => processor.Process(() => mediaControls.GetStatusAsync()));
            
            var thumbnailCommand = new Command("thumbnail");
            thumbnailCommand.SetHandler(() => processor.Process(() => mediaControls.GetThumbnailAsync()));
            
            Add(statusCommand);
            Add(thumbnailCommand);
            Add(skipNextCommand);
            Add(skipPreviousCommand);
            Add(pauseCommand);
            Add(playCommand);
            Add(pauseOrPlayCommand);
        }
    }
}