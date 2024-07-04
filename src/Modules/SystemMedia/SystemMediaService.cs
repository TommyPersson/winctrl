using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Control;
using winctrl.Modules.SystemMedia.Contracts;

namespace winctrl.Modules.SystemMedia
{
    public class SystemMediaService
    {
        public async Task<object> PauseAsync()
        {
            var session = await GetSessionAsync();
            await session.TryPauseAsync();
            return null;
        }
        
        public async Task<object> PlayAsync()
        {
            var session = await GetSessionAsync();
            await session.TryPlayAsync();
            return null;
        }

        public async Task<object> ResumeAsync()
        {
            var session = await GetSessionAsync();
            await session.TryPlayAsync();
            return null;
        }

        public async Task<object> PauseOrResumeAsync()
        {
            var session = await GetSessionAsync();
            await session.TryTogglePlayPauseAsync();
            return null;
        }
        
        public async Task<object> SkipNextAsync()
        {
            var session = await GetSessionAsync();
            await session.TrySkipNextAsync();
            return null;
        }
        
        public async Task<object> SkipPreviousAsync()
        {
            var session = await GetSessionAsync();
            await session.TrySkipPreviousAsync();
            return null;
        }

        public async Task<StatusDTO> GetStatusAsync()
        {
            var session = await GetSessionAsync();
            return new StatusDTO
            {
                PlaybackInfo = session.GetPlaybackInfo(),
                TimelineProperties = session.GetTimelineProperties(),
                MediaProperties = await session.TryGetMediaPropertiesAsync()
            };
        }

        public async Task<ThumbnailDTO> GetThumbnailAsync()
        {
            var session = await GetSessionAsync();
            var properties = await session.TryGetMediaPropertiesAsync();
            var thumbnailStream = await properties.Thumbnail.OpenReadAsync();
            var bytesStream = new MemoryStream();
            thumbnailStream.AsStreamForRead().CopyTo(bytesStream);
            var bytes = bytesStream.ToArray();

            return new ThumbnailDTO()
            {
                ContentType = thumbnailStream.ContentType,
                Bytes = bytes,
            };
        }

        private async Task<GlobalSystemMediaTransportControlsSession> GetSessionAsync()
        {
            var manager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
            var session = manager.GetCurrentSession();
            if (session == null)
            {
                throw new NoMediaSessionAvailableException();
            }
            return session;
        }
    }

    public class NoMediaSessionAvailableException : Exception
    {
    }
}