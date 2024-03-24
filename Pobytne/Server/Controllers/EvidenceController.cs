using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural.FilterReports;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using Pobytne.Shared.Extensions;
using System.Net;

namespace Pobytne.Server.Controllers
{
	[Route("Evidence")]
	[Authorize]
	[ApiController]
	public class EvidenceController(InteractionService interactionService) : ControllerBase
	{
        private readonly InteractionService _interactionService = interactionService;
		private const EPermition permition = EPermition.EvidenceSummary;

        [HttpPost]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		[Route("Filter")]
		public async Task<IActionResult> GetFiltered(EvidenceFilter filter)
		{
            try
            {
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
