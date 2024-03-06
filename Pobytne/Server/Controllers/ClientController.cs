using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Controllers
{
    [Route("Client")]
    [Authorize]
    [ApiController]
    public class ClientController : ControllerBase
	{
        private readonly ClientService _clientService;
        public const EPermition permition = EPermition.Client; 
        public ClientController(ClientService clientService)
        {
            _clientService = clientService;
        }
        [HttpGet]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		[Route("ClientsList")]
        public Task<IEnumerable<Shared.Procedural.DTO.Client>> GetClients(int moduleNumber)
        {
            return _clientService.GetClientsByModule(moduleNumber);
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Shared.Procedural.DTO.Client updateClient)
        {/////////////////
            if (updateClient is null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                int rowsAffected = await _clientService.Update(updateClient);
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
        // POST api/<ItemsController>
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Insert")]
        public async Task<IActionResult> Insert([FromBody] Shared.Procedural.DTO.Client insertClient)
        {
            if (insertClient is null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                int? rowsAffected = await _clientService.Insert(insertClient);
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
