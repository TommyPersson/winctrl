namespace winctrl.Modules.Executables.Contracts
{
    public class ExecutableIconDTO
    {
        public string ExecutableFilePath { get; set; }

        public int Index { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string ContentType { get; set; }

        public byte[] Bytes { get; set; }
    }
}