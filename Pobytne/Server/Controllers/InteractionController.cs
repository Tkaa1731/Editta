using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Net;

namespace Pobytne.Server.Controllers
{
    [Route("Interaction")]
    [Authorize]
    [ApiController]
    public class InteractionController(InteractionService interactionService) : ControllerBase
    {
        private readonly InteractionService _interactionService = interactionService;
        private const EPermition permition = EPermition.Evidence;

        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Insert([FromBody] Interaction insertInteraction)
        {
            if (insertInteraction is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                int rowsAffected = await _interactionService.Insert(insertInteraction);
                if (rowsAffected > 0)
                    return NoContent();

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se vložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
