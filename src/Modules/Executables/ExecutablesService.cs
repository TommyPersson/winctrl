using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
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
            // Icon and Bitmap wrap GDI handles; they must be disposed or the process runs out of GDI objects.
            using (var icon = Icon.ExtractAssociatedIcon(filePath))
            {
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
        }

        public Task<ExecutableIconDTO> GetIconData(string filePath, int index, string size)
        {
            using (var icon = Icon.ExtractAssociatedIcon(filePath))
            using (var bitmap = icon.ToBitmap())
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return Task.FromResult(new ExecutableIconDTO()
                {
                    ExecutableFilePath = filePath,
                    Index = 0,
                    Height = icon.Height,
                    Width = icon.Width,
                    ContentType = "image/png",
                    Bytes = stream.ToArray(),
                });
            }
        }
    }
}
