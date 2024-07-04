using Windows.Media.Control;

namespace winctrl.Modules.SystemMedia.Contracts
{
    public class StatusDTO
    {
        public GlobalSystemMediaTransportControlsSessionPlaybackInfo PlaybackInfo { get; set; }

        public GlobalSystemMediaTransportControlsSessionTimelineProperties TimelineProperties { get; set; }

        public GlobalSystemMediaTransportControlsSessionMediaProperties MediaProperties { get; set; }
    }
}