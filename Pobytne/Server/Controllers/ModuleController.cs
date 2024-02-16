using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Controllers
{

	[Route("Module")]
  [ApiController]
  [Authorize]
	public class ModuleController : ControllerBase
	{
		private ModuleService _moduleService;
		public const EPermition permition = EPermition.Module;

		public ModuleController(ModuleService moduleService)
		{
			_moduleService = moduleService;
		}
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [HttpGet]
		public async Task<IEnumerable<Module>> Get([FromQuery]int licenseNumber)
        { 
			return await _moduleService.GetModulesByLicense(licenseNumber);
		}
		[HttpGet]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("ModulesOfUser")]
		public async Task<IEnumerable<Module>> ModulesOfUser(int userId)
		{
			return await _moduleService.GetModulesByUser(userId);
		}
		[HttpPost]
		[PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Module updateModule)
        {
            if (updateModule is null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                int rowsAffected = await _moduleService.Update(updateModule);
                if (rowsAffected > 0)
                    return Ok("Update successful");
                else
                    return BadRequest("Update failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :{ex.Message}");
            }
        }

        // DELETE api/<ItemsController>/5
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            var lines = _moduleService.Delete(id).Result;
            if (lines == 1)
                return Ok();
            return BadRequest();
        }
        // POST api/<ItemsController>
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Insert")]
        public async Task<IActionResult> Insert([FromBody] Module insertModule)
        {
            if (insertModule is null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                int? rowsAffected = await _moduleService.Insert(insertModule);
                if (rowsAffected > 0)
                    return Ok("Insert successful");
                else
                    return BadRequest("Insert failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :{ex.Message}");
            }
        }

    }
}
