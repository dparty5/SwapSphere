using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_SwapSphere_dblayer;
using project_SwapSphere_models.Models;
using project_SwapSphere_models;
using project_SwapSphere_server.Service;

namespace project_SwapSphere_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly EntityGateway _db = new();
        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ? Request.Headers["Token"]! : Guid.Empty.ToString());


        [HttpGet]
        [Route("{id}/orders")]
        public IActionResult GetOrdersInClient([FromRoute] Guid id)
        {
            var potentialClient = _db.GetClients(x => x.Id == id).FirstOrDefault();
            return potentialClient is null ?
                   NotFound(new
                   {
                       status = "fail",
                       message = $"There is no client with this id {id}!"
                   }) :
                   Ok(new
                   {
                       status = "ok",
                       orders = potentialClient.Orders
                   });
        }

     

        [HttpPost]
        public IActionResult Post([FromBody] Client value)
        {
            try
            {
                if (LocalAuthService.GetInstance().GetRole(Token) != Role.Admin)
                    return Unauthorized(new
                    {
                        status = "fail",
                        message = "You have no rights for this op."
                    });
                _db.AddOrUpdate(value);
                return Ok(new
                {
                    status = "ok",
                    id = value.Id
                });
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
