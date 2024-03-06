using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;

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
        public async Task<IEnumerable<Permition>> GetByModule([FromQuery] int idModule)
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
                return BadRequest("Invalid data");
            }
            try
            {
                int? rowsAffected = await _permitionService.Insert(insertPermition);
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
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Permition updatePermition)
        {
            if (updatePermition is null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                int rowsAffected = await _permitionService.Update(updatePermition);
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
    }
}
