using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Controllers
{
    [Route("Permition")]
    [ApiController]
    [Authorize]
    public class PermitionController : ControllerBase
    {
        private readonly UserService _userService;
        public const EPermition permition = EPermition.Permition;

        public PermitionController(UserService userService)
        {
            _userService = userService;
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
                int? rowsAffected = await _userService.Insert(insertPermition);
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
                int rowsAffected = await _userService.Update(updatePermition);
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
