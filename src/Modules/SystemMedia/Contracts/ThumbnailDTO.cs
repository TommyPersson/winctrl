using System;

namespace winctrl.Modules.SystemMedia.Contracts
{
    public class ThumbnailDTO
    {
        public String ContentType { get; set; }

        public byte[] Bytes { get; set; }
    }
}