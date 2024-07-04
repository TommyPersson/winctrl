using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using winctrl.Modules.Executables.Contracts;

namespace winctrl.Modules.Executables
{
    public class ExecutablesService
    {
        // TODO get all icons
        // TODO get jumbo icons

        public Task<ExecutableIconsDTO> GetIconsForFile(string filePath)
        {
            var icon = Icon.ExtractAssociatedIcon(filePath);
            return Task.FromResult(new ExecutableIconsDTO
            {
                ExecutableFilePath = filePath,
                Icons = new List<ExecutableIconsDTO.IconInfo>
                {
                    new ExecutableIconsDTO.IconInfo()
                    {
                        Index = 0,
                        Sizes = new List<string> { $"{icon.Width}x{icon.Height}" }
                    },
                }
            });
        }

        public Task<ExecutableIconDTO> GetIconData(string filePath, int index, string size)
        {
            var  icon = Icon.ExtractAssociatedIcon(filePath);
            var stream = new MemoryStream();
            icon.ToBitmap().Save(stream, ImageFormat.Png);
            var bytes = stream.ToArray();
            return Task.FromResult(new ExecutableIconDTO()
            {
                ExecutableFilePath = filePath,
                Index = 0,
                Height = icon.Height,
                Width = icon.Width,
                ContentType = "image/png",
                Bytes = bytes,
            });
        }
    }
}