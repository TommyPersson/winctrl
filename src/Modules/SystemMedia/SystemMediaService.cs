using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.Media.Control;
using winctrl.Modules.SystemMedia.Contracts;

namespace winctrl.Modules.SystemMedia
{
    public class SystemMediaService
    {
        // One manager for the lifetime of the process. Calling RequestAsync() per request registers a new
        // manager with the media broker every time, which accumulates there when polled continuously.
        private static readonly SemaphoreSlim ManagerLock = new SemaphoreSlim(1, 1);
        private static GlobalSystemMediaTransportControlsSessionManager _manager;

        public Task<object> PauseAsync() => WithSessionAsync(s => s.TryPauseAsync().AsTask());

        public Task<object> PlayAsync() => WithSessionAsync(s => s.TryPlayAsync().AsTask());

        public Task<object> ResumeAsync() => WithSessionAsync(s => s.TryPlayAsync().AsTask());

        public Task<object> PauseOrResumeAsync() => WithSessionAsync(s => s.TryTogglePlayPauseAsync().AsTask());

        public Task<object> SkipNextAsync() => WithSessionAsync(s => s.TrySkipNextAsync().AsTask());

        public Task<object> SkipPreviousAsync() => WithSessionAsync(s => s.TrySkipPreviousAsync().AsTask());

        public async Task<StatusDTO> GetStatusAsync()
        {
            var session = await GetSessionAsync();
            try
            {
                return new StatusDTO
                {
                    PlaybackInfo = ToDto(session.GetPlaybackInfo()),
                    TimelineProperties = ToDto(session.GetTimelineProperties()),
                    MediaProperties = ToDto(await session.TryGetMediaPropertiesAsync()),
                };
            }
            finally
            {
                Release(session);
            }
        }

        public async Task<ThumbnailDTO> GetThumbnailAsync()
        {
            var session = await GetSessionAsync();
            GlobalSystemMediaTransportControlsSessionMediaProperties properties = null;
            try
            {
                properties = await session.TryGetMediaPropertiesAsync();
                var thumbnailRef = properties?.Thumbnail;
                if (thumbnailRef == null)
                {
                    return null;
                }

                try
                {
                    using (var thumbnailStream = await thumbnailRef.OpenReadAsync())
                    using (var readStream = thumbnailStream.AsStreamForRead())
                    using (var bytesStream = new MemoryStream())
                    {
                        await readStream.CopyToAsync(bytesStream);
                        return new ThumbnailDTO
                        {
                            ContentType = thumbnailStream.ContentType,
                            Bytes = bytesStream.ToArray(),
                        };
                    }
                }
                finally
                {
                    Release(thumbnailRef);
                }
            }
            finally
            {
                Release(properties);
                Release(session);
            }
        }

        private async Task<object> WithSessionAsync(Func<GlobalSystemMediaTransportControlsSession, Task<bool>> action)
        {
            var session = await GetSessionAsync();
            try
            {
                await action(session);
                return null;
            }
            finally
            {
                Release(session);
            }
        }

        private static async Task<GlobalSystemMediaTransportControlsSession> GetSessionAsync()
        {
            var manager = await GetManagerAsync();
            GlobalSystemMediaTransportControlsSession session;
            try
            {
                session = manager.GetCurrentSession();
            }
            catch (COMException)
            {
                // The broker may have restarted (e.g. explorer restart); drop the stale manager and retry once.
                await ResetManagerAsync(manager);
                manager = await GetManagerAsync();
                session = manager.GetCurrentSession();
            }

            if (session == null)
            {
                throw new NoMediaSessionAvailableException();
            }

            return session;
        }

        private static async Task<GlobalSystemMediaTransportControlsSessionManager> GetManagerAsync()
        {
            var manager = Volatile.Read(ref _manager);
            if (manager != null)
            {
                return manager;
            }

            await ManagerLock.WaitAsync();
            try
            {
                if (_manager == null)
                {
                    _manager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
                }

                return _manager;
            }
            finally
            {
                ManagerLock.Release();
            }
        }

        private static async Task ResetManagerAsync(GlobalSystemMediaTransportControlsSessionManager stale)
        {
            await ManagerLock.WaitAsync();
            try
            {
                if (ReferenceEquals(_manager, stale))
                {
                    _manager = null;
                    Release(stale);
                }
            }
            finally
            {
                ManagerLock.Release();
            }
        }

        private static PlaybackInfoDTO ToDto(GlobalSystemMediaTransportControlsSessionPlaybackInfo info)
        {
            if (info == null) return null;
            try
            {
                return new PlaybackInfoDTO
                {
                    AutoRepeatMode = info.AutoRepeatMode,
                    Controls = ToDto(info.Controls),
                    IsShuffleActive = info.IsShuffleActive,
                    PlaybackRate = info.PlaybackRate,
                    PlaybackStatus = info.PlaybackStatus,
                    PlaybackType = info.PlaybackType,
                };
            }
            finally
            {
                Release(info);
            }
        }

        private static PlaybackControlsDTO ToDto(GlobalSystemMediaTransportControlsSessionPlaybackControls c)
        {
            if (c == null) return null;
            try
            {
                return new PlaybackControlsDTO
                {
                    IsChannelDownEnabled = c.IsChannelDownEnabled,
                    IsChannelUpEnabled = c.IsChannelUpEnabled,
                    IsFastForwardEnabled = c.IsFastForwardEnabled,
                    IsNextEnabled = c.IsNextEnabled,
                    IsPauseEnabled = c.IsPauseEnabled,
                    IsPlayEnabled = c.IsPlayEnabled,
                    IsPlayPauseToggleEnabled = c.IsPlayPauseToggleEnabled,
                    IsPlaybackPositionEnabled = c.IsPlaybackPositionEnabled,
                    IsPlaybackRateEnabled = c.IsPlaybackRateEnabled,
                    IsPreviousEnabled = c.IsPreviousEnabled,
                    IsRecordEnabled = c.IsRecordEnabled,
                    IsRepeatEnabled = c.IsRepeatEnabled,
                    IsRewindEnabled = c.IsRewindEnabled,
                    IsShuffleEnabled = c.IsShuffleEnabled,
                    IsStopEnabled = c.IsStopEnabled,
                };
            }
            finally
            {
                Release(c);
            }
        }

        private static TimelinePropertiesDTO ToDto(GlobalSystemMediaTransportControlsSessionTimelineProperties t)
        {
            if (t == null) return null;
            try
            {
                return new TimelinePropertiesDTO
                {
                    EndTime = t.EndTime,
                    LastUpdatedTime = t.LastUpdatedTime,
                    MaxSeekTime = t.MaxSeekTime,
                    MinSeekTime = t.MinSeekTime,
                    Position = t.Position,
                    StartTime = t.StartTime,
                };
            }
            finally
            {
                Release(t);
            }
        }

        private static MediaPropertiesDTO ToDto(GlobalSystemMediaTransportControlsSessionMediaProperties p)
        {
            if (p == null) return null;
            try
            {
                var genres = p.Genres;
                List<string> genreList;
                try
                {
                    genreList = genres?.ToList() ?? new List<string>();
                }
                finally
                {
                    Release(genres);
                }

                return new MediaPropertiesDTO
                {
                    AlbumArtist = p.AlbumArtist,
                    AlbumTitle = p.AlbumTitle,
                    AlbumTrackCount = p.AlbumTrackCount,
                    Artist = p.Artist,
                    Genres = genreList,
                    PlaybackType = p.PlaybackType,
                    Subtitle = p.Subtitle,
                    Title = p.Title,
                    TrackNumber = p.TrackNumber,
                };
            }
            finally
            {
                Release(p);
            }
        }

        // Releases one reference to the RCW now instead of waiting for the finalizer. With very little managed
        // allocation, GC runs rarely and the remote references held by these proxies would otherwise pile up.
        // ReleaseComObject (not FinalReleaseComObject) is used, so the same RCW handed out concurrently to
        // another request (e.g. the current session) stays valid.
        private static void Release(object o)
        {
            if (o != null && Marshal.IsComObject(o))
            {
                Marshal.ReleaseComObject(o);
            }
        }
    }

    public class NoMediaSessionAvailableException : Exception
    {
    }
}
