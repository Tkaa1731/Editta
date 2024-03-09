using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using System.Net;

namespace Pobytne.Server.Controllers
{
    [Route("Permition")]
    [ApiController]
    [Authorize]
    public class PermitionController(PermitionService permitionService) : ControllerBase
    {
        private readonly PermitionService _permitionService = permitionService;
        public const EPermition permition = EPermition.Permition;


        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("PermitionList")]
        public async Task<IEnumerable<Permition>> Get([FromQuery] int idModule)
        {
            return await _permitionService.GetAllOfModule(idModule);
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Insert")]
        public async Task<IActionResult> Insert([FromBody] Permition insertPermition)
        {
            if (insertPermition is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "No data to insert"));
			}
            try
            {
                await _permitionService.Insert(insertPermition);
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.Conflict, ex.Message));
			}
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Permition updatePermition)
        {
            if (updatePermition is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "No data to insert"));
			}
            try
            {
                await _permitionService.Update(updatePermition);
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.Conflict, ex.Message));
			}
        }
    }
}
