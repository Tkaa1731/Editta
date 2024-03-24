using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.FilterReports;
using Pobytne.Shared.Struct;
using System.Net;

namespace Pobytne.Server.Controllers
{
    [Route("CashRegister")]
	[Authorize]
	[ApiController]
	public class CashRegisterController(InteractionService interactionService) : ControllerBase
	{
		private readonly InteractionService _interactionService = interactionService;
		private const EPermition permition = EPermition.CashSummary;

        [HttpGet]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		public async Task<IActionResult> Get([FromQuery] string filterJSON)
		{
            try
            {
                var filter = JsonConvert.DeserializeObject<CashRegisterFilter>(filterJSON);
                if(filter is null)
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
