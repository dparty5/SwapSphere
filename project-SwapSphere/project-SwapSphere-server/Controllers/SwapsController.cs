using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project_SwapSphere_dblayer;
using project_SwapSphere_models;
using project_SwapSphere_models.Models;
using project_SwapSphere_server.Service;
using static project_SwapSphere_dblayer.EntityGateway;

namespace project_SwapSphere_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SwapsController : ControllerBase
    {
        private readonly EntityGateway _db = new();
        private Guid Token => Guid.Parse(Request.Headers["Token"] != string.Empty ? Request.Headers["Token"]! : Guid.Empty.ToString());

        /// <summary>
        /// Get Swap news
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/news")]
        public IActionResult GetNewsSwap()
        {
            return Ok();
        }

        /// <summary>
        /// Get all swaps
        /// </summary>
        /// <returns></returns>
        /// 

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(new
            {
                status = "ok",
                swaps = _db.GetSwaps()
            });
        }

        /// <summary>
        /// Get swap by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(Guid id)
        {
            var potentialSwap = _db.GetSwaps(x => x.Id == id).FirstOrDefault();
            if (potentialSwap is not null)
                return Ok(new
                {
                    status = "ok",
                    swap = potentialSwap,
                    entities = potentialSwap.Keys_Swap.Select(x => x.Id),
                    category = potentialSwap.Category
                });
            else
                return NotFound(new
                {
                    status = "fail",
                    message = $"There is no swaps with this id {id}! "
                });
        }

        [HttpGet]
        [Route("{id}/subdata")]
        public IActionResult GetSubdataFromSwap([FromRoute] Guid id, [FromRoute] SwapSubdata subdata)
        {
            var potentialSwap = _db.GetSwaps(x => x.Id == id).FirstOrDefault();
            object res = subdata switch
            {
                SwapSubdata.Entities => new
                {
                    status = "ok",
                    employees = potentialSwap?.Keys_Swap
                },
                SwapSubdata.UsedCategory => new
                {
                    status = "ok",
                    usedcategory = potentialSwap?.Category
                },
                _ => throw new Exception($"{subdata} instance is not covered.")
            };
            return potentialSwap is null
                ? NotFound(new
                {
                    status = "fail",
                    message = $"There is no swap with this id {id}!"
                })
                : Ok(res);

        }

        [HttpPost]
        public IActionResult PostSwap([FromBody] Swap swap)
        {
            if (LocalAuthService.GetInstance().GetRole(Token) != Role.Admin)
                return Unauthorized(new
                {
                    status = "fail",
                    message = "You have no rights for that action."
                });

            _db.AddOrUpdate(swap);
            return Ok(new
            {
                status = "ok",
                id = swap.Id
            });
        }
        /// <summary>
        ///  change subdataIds from swap
        /// </summary>
        /// <param name="action"></param>
        /// <param name="id"></param>
        /// <param name="subdataIds">Json array of subdataIds id</param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}/entities/add")]
        [Route("{id}/{subdata}/{action}")]
        public IActionResult ManipulateSubDataInProject([FromRoute] ActionType action,
                                                        [FromRoute] SwapSubdata subdata,
                                                        [FromRoute] Guid id,
                                                        [FromBody] Guid[] subdataIds)
        {
            try
            {
                if (LocalAuthService.GetInstance().GetRole(Token) != Role.Admin)
                    return Unauthorized(new
                    {
                        status = "fail",
                        message = "You have no rights for this op."
                    });

                var changed = subdata switch
                {
                    SwapSubdata.Entities => _db.EntitiesInSwap(action, id, subdataIds),
                    SwapSubdata.UsedCategory => _db.SwapsInCategory(action, id, subdataIds),
                    _ => throw new Exception($"{subdata} instance is not covered.")
                };
                return Ok(new
                {
                    status = "ok",
                    changed
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


#pragma warning disable CS1591
        public enum SwapSubdata
        {
            UsedCategory,
            Entities
        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
