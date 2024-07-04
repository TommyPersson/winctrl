using System.Collections.Generic;

namespace winctrl.Modules.Executables.Contracts
{
    public class ExecutableIconsDTO
    {
        public string ExecutableFilePath { get; set; }

        public List<IconInfo> Icons { get; set; }

        public class IconInfo
        {
            public int Index { get; set; }

            public List<string> Sizes { get; set; }
        }
    }
}