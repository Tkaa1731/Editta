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
        public async Task<IActionResult> Get([FromQuery] int moduleId, [FromQuery] string filterJSON = "")
        {
            try
            {
                var permitions = await _permitionService.GetAllOfModule(moduleId);
                return Ok(permitions);
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPut]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Update([FromBody] Permition updatePermition)
        {
            if (updatePermition is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var updatedPermition = await _permitionService.Update(updatePermition);
                if (updatedPermition is not null)
                    return Ok(updatedPermition);

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se uložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Insert([FromBody] Permition insertPermition)
        {
            if (insertPermition is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var insertedPermition = await _permitionService.Insert(insertPermition);
                if (insertedPermition is not null)
                    return Ok(insertedPermition);

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se vložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpDelete("{id}")]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _permitionService.Delete(id);
                if (result > 0)
                    return NoContent();

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se smazat záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
