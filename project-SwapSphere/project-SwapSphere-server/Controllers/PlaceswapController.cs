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
    public class PlaceswapController : ControllerBase
    {
        private readonly EntityGateway _db = new();
        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ? Request.Headers["Token"]! : Guid.Empty.ToString());


        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new
            {
                status = "ok",
                swaps = _db.GetPlaceswap()
            });
        }

        [HttpGet]
        [Route("{id}/swaps")]
        public IActionResult GetSwapInPlaceswap([FromRoute] Guid id)
        {
            var potentialPlatform = _db.GetPlaceswap(x => x.Id == id).FirstOrDefault();
            return potentialPlatform is null ?
                   NotFound(new
                   {
                       status = "fail",
                       message = $"There is no platform with this id {id}!"
                   }) :
                   Ok(new
                   {
                       status = "ok",
                       swaps = potentialPlatform.Swaps
                   });
        }

        [HttpPost]
        public IActionResult Post([FromBody] Placeswap value)
        {
            try
            {
                if (LocalAuthService.GetInstance().GetRole(Token) != Role.User)
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
