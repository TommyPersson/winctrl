using System.Threading.Tasks;
using System.Web.Http;

namespace winctrl.Modules.Keyboard
{
    [RoutePrefix("api/keyboard")]
    public class KeyboardApiController : ApiController
    {
        KeyboardService _keyboardService = new KeyboardService();

        [Route("actions/send-keys")]
        [HttpPost]
        public async Task<IHttpActionResult> SendKeys()
        {
            var keys = await Request.Content.ReadAsStringAsync();
            await _keyboardService.SendKeys(keys);
            return Ok();
        }
    }
}