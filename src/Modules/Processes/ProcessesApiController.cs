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
    }
}