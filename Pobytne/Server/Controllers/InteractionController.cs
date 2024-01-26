using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural;

namespace Pobytne.Server.Controllers
{
    [Route("Interaction")]
    [Authorize]
    [ApiController]
    public class InteractionController : ControllerBase
    {
        private readonly InteractionService _interactionService;
        public InteractionController(InteractionService interactionService) => _interactionService = interactionService;
        // POST api/<ItemsController>
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Insert([FromBody] Interaction insertInteraction)
        {
            if (insertInteraction is null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                int? rowsAffected = await _interactionService.Insert(insertInteraction);
                if (rowsAffected is not null)
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
