using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using System.Net;

namespace Pobytne.Server.Controllers
{

    [Route("Module")]
    [ApiController]
    [Authorize]
	public class ModuleController(ModuleService moduleService) : ControllerBase
	{
		private readonly ModuleService _moduleService = moduleService;
		public const EPermition permition = EPermition.Module;

        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
		public async Task<IActionResult> Get([FromQuery]int licenseNumber = -1, [FromQuery] int userId = -1)
        {
            try
            {
                if (licenseNumber > 0)
                {
                    var modules = await _moduleService.GetModulesByLicense(licenseNumber);
                    return Ok(modules);
                }
                if(userId > 0)
                {
                    var modules = await _moduleService.GetModulesByUser(userId);
                    return Ok(modules);
                }

                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
		}

		[HttpPut]
		[PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Update([FromBody] Module updateModule)
        {
            if (updateModule is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var updatedModule = await _moduleService.Update(updateModule);
                if (updatedModule is not null)
                    return Ok(updatedModule);
                
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se uložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Insert([FromBody] Module insertModule)
        {
            if (insertModule is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var insertedModule = await _moduleService.Insert(insertModule);
                if (insertedModule is not null)
                    return Ok(insertedModule);

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
                var result = await _moduleService.Delete(id);
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
