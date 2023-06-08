using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_SwapSphere_dblayer;
using project_SwapSphere_server.Service;

namespace project_SwapSphere_server.Controllers
{
    [Route("api/projects/{project_id}/chat")]
    [ApiController]
    public class ChatController : ControllerBase
    {
       readonly LocalAuthService _localAuthService = LocalAuthService.GetInstance();
        readonly EntityGateway _db = new();

        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ? Request.Headers["Token"] : Guid.Empty.ToString());

        readonly ChatService _chatService = ChatService.GetInstance();

        [HttpGet]
        public async Task<IActionResult> ConnectUser([FromRoute] Guid swap_id, Guid token)
        {
            var potentialSwap = _db.GetSwaps(x => x.Id == swap_id).FirstOrDefault();
            if (potentialSwap is null)
                return NotFound(new
                {
                    status = "fail",
                    message = "There is no swap with this Id"
                });
            if (!ControllerContext.HttpContext.WebSockets.IsWebSocketRequest)          
                return BadRequest(new
                {
                    status = "fail",
                    message = "Unsupported action!"
                });
            var soket = await ControllerContext.HttpContext.WebSockets.AcceptWebSocketAsync();
            try
            {
                var user = _localAuthService.GetClient(token);
                await Task.Delay(TimeSpan.FromMilliseconds(-1), _chatService.CreateConnection(user,potentialSwap, soket));
                return Ok();
            }
            catch (Exception E)
            {
                return BadRequest(new
                {
                    status = "fail",
                    message = E.Message
                });
            }
        }
    }
}
