using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural.FilterReports;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using Pobytne.Shared.Extensions;
using System.Net;
using Newtonsoft.Json;

namespace Pobytne.Server.Controllers
{
	[Route("Evidence")]
	[Authorize]
	[ApiController]
	public class EvidenceController(InteractionService interactionService) : ControllerBase
	{
        private readonly InteractionService _interactionService = interactionService;
		private const EPermition permition = EPermition.EvidenceSummary;

		[HttpGet]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		public async Task<IActionResult> Get([FromQuery] string filterJSON)
		{
            try
            {
				var filter = JsonConvert.DeserializeObject<EvidenceFilter>(filterJSON);
				if (filter is null)
					return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));

				var reports = await _interactionService.GetFilteredReports(filter);
                return Ok(reports);
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
		}
	}
}
