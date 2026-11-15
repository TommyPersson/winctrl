using System.Threading.Tasks;
using System.Web.Http;

namespace winctrl.Modules.Processes
{
    [RoutePrefix("api/processes")]
    public class ProcessesApiController : ApiController
    {
        ProcessesService _processesService = new ProcessesService();

        [Route("active-window-info")]
        [HttpGet]
        public async Task<IHttpActionResult> GetActiveWindowInfo()
        {
            var info = await _processesService.GetActiveWindowInfoAsync();
            return Ok(info);
        }
        
        [Route("actions/start-process")]
        [HttpPost]
        public async Task<IHttpActionResult> StartProcess()
        {
            var executable = await Request.Content.ReadAsStringAsync();
            await _processesService.StartProcess(executable);
            return Ok();
        }
        
        [Route("actions/open-url")]
        [HttpPost]
        public async Task<IHttpActionResult> OpenUrl()
        {
            var url = await Request.Content.ReadAsStringAsync();
            await _processesService.OpenUrl(url);
            return Ok();
        }

        /// <summary>
        /// Brings the main window of the process named in the body (e.g. "claude") to the front.
        /// 404 if no such process has a window.
        /// </summary>
        [Route("actions/focus-window")]
        [HttpPost]
        public async Task<IHttpActionResult> FocusWindow()
        {
            var processName = await Request.Content.ReadAsStringAsync();
            var focused = await _processesService.FocusWindow(processName);
            if (focused == null)
            {
                return NotFound();
            }

            return Ok(new { Focused = focused.Value });
        }
    }
}