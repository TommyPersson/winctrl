using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Http;

namespace winctrl.Modules.Executables
{
    [RoutePrefix("api/executables")]
    public class ExecutablesApiController : ApiController
    {
        ExecutablesService _executablesService = new ExecutablesService();

        [Route("icon-data")]
        [HttpGet]
        public async Task<IHttpActionResult> GetIconData(
            string filePath,
            int index = 0,
            string size = ""
        )
        {
            var info = await _executablesService.GetIconData(filePath, index, size);
            return Ok(info);
        }
        
        [Route("icon-image")]
        [HttpGet]
        public async Task<IHttpActionResult> GetIconImage(
            string filePath,
            int index = 0,
            string size = ""
        )
        {
            var info = await _executablesService.GetIconData(filePath, index, size);
            
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            response.Content = new ByteArrayContent(info.Bytes);
            response.Content.Headers.ContentType = new MediaTypeHeaderValue(info.ContentType);

            return ResponseMessage(response);
        }
        
        [Route("icons")]
        [HttpGet]
        public async Task<IHttpActionResult> GetIconsForFile(string filePath)
        {
            var info = await _executablesService.GetIconsForFile(filePath);
            return Ok(info);
        }
    }
}