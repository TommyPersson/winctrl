namespace winctrl.Modules.Processes.Contracts
{
    public class ActiveWindowInfoDTO
    {
        public string WindowTitle { get; set; }
        
        public string ExecutableName { get; set; }
        
        public string ExecutableFileName { get; set; }
        
        public string ProcessName { get; set; }
        
        public int ProcessId { get; set; }
    }
}