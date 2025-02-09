using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace winctrl.Modules.SystemMedia
{
    [RoutePrefix("api/system-media")]
    public class SystemMediaApiController : ApiController
    {
        SystemMediaService _systemMediaService = new SystemMediaService();

        [Route("status")]
        [HttpGet]
        public async Task<IHttpActionResult> GetStatus()
        {
            var status = await _systemMediaService.GetStatusAsync();
            return Ok(status);
        }

        [Route("thumbnail")]
        [HttpGet]
        public async Task<IHttpActionResult> GetThumbnail()
        {
            var thumbnail = await _systemMediaService.GetThumbnailAsync();
            if (thumbnail == null)
            {
                return NotFound();
            }

            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(thumbnail.Bytes);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(thumbnail.ContentType);

            return ResponseMessage(response);
        }

        [Route("actions/skip-next")]
        [HttpPost]
        public async Task<IHttpActionResult> SkipNext()
        {
            await _systemMediaService.SkipNextAsync();
            return Ok();
        }

        [Route("actions/skip-previous")]
        [HttpPost]
        public async Task<IHttpActionResult> SkipPrevious()
        {
            await _systemMediaService.SkipPreviousAsync();
            return Ok();
        }

        [Route("actions/pause")]
        [HttpPost]
        public async Task<IHttpActionResult> Pause()
        {
            await _systemMediaService.PauseAsync();
            return Ok();
        }

        [Route("actions/play")]
        [HttpPost]
        public async Task<IHttpActionResult> Play()
        {
            await _systemMediaService.PlayAsync();
            return Ok();
        }

        [Route("actions/resume")]
        [HttpPost]
        public async Task<IHttpActionResult> Resume()
        {
            await _systemMediaService.ResumeAsync();
            return Ok();
        }

        [Route("actions/pause-or-resume")]
        [HttpPost]
        public async Task<IHttpActionResult> PauseOrResume()
        {
            await _systemMediaService.PauseOrResumeAsync();
            return Ok();
        }
    }
}