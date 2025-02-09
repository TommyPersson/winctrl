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
    }
}