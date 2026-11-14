using System;
using System.Collections.Generic;
using Windows.Media;
using Windows.Media.Control;

namespace winctrl.Modules.SystemMedia.Contracts
{
    // Plain snapshots of the WinRT objects. Serializing the WinRT objects directly would keep
    // COM proxies (and their references in the media broker process) alive until finalization.
    public class StatusDTO
    {
        public PlaybackInfoDTO PlaybackInfo { get; set; }

        public TimelinePropertiesDTO TimelineProperties { get; set; }

        public MediaPropertiesDTO MediaProperties { get; set; }
    }

    public class PlaybackInfoDTO
    {
        public MediaPlaybackAutoRepeatMode? AutoRepeatMode { get; set; }
        public PlaybackControlsDTO Controls { get; set; }
        public bool? IsShuffleActive { get; set; }
        public double? PlaybackRate { get; set; }
        public GlobalSystemMediaTransportControlsSessionPlaybackStatus PlaybackStatus { get; set; }
        public MediaPlaybackType? PlaybackType { get; set; }
    }

    public class PlaybackControlsDTO
    {
        public bool IsChannelDownEnabled { get; set; }
        public bool IsChannelUpEnabled { get; set; }
        public bool IsFastForwardEnabled { get; set; }
        public bool IsNextEnabled { get; set; }
        public bool IsPauseEnabled { get; set; }
        public bool IsPlayEnabled { get; set; }
        public bool IsPlayPauseToggleEnabled { get; set; }
        public bool IsPlaybackPositionEnabled { get; set; }
        public bool IsPlaybackRateEnabled { get; set; }
        public bool IsPreviousEnabled { get; set; }
        public bool IsRecordEnabled { get; set; }
        public bool IsRepeatEnabled { get; set; }
        public bool IsRewindEnabled { get; set; }
        public bool IsShuffleEnabled { get; set; }
        public bool IsStopEnabled { get; set; }
    }

    public class TimelinePropertiesDTO
    {
        public TimeSpan EndTime { get; set; }
        public DateTimeOffset LastUpdatedTime { get; set; }
        public TimeSpan MaxSeekTime { get; set; }
        public TimeSpan MinSeekTime { get; set; }
        public TimeSpan Position { get; set; }
        public TimeSpan StartTime { get; set; }
    }

    public class MediaPropertiesDTO
    {
        public string AlbumArtist { get; set; }
        public string AlbumTitle { get; set; }
        public int AlbumTrackCount { get; set; }
        public string Artist { get; set; }
        public List<string> Genres { get; set; }
        public MediaPlaybackType? PlaybackType { get; set; }
        public string Subtitle { get; set; }
        public string Title { get; set; }
        public int TrackNumber { get; set; }
    }
}
