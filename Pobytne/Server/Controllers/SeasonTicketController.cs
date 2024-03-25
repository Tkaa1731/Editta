using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.Filters;
using Pobytne.Shared.Struct;
using System.Net;

namespace Pobytne.Server.Controllers
{
	[Route("SeasonTicket")]
	[Authorize]
	[ApiController]
	public class SeasonTicketController(SeasonTicketService service) : ControllerBase
	{
		private readonly SeasonTicketService _service = service;
		public const EPermition permition = EPermition.SeasonTicketEvidence;

		[HttpGet]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		public async Task<IActionResult> Get([FromQuery] int recordId)
		{
			try
			{
				var tickets = await _service.GetSeasonTicketEvidence(recordId);
				return Ok(tickets);
			}
			catch (Exception ex)
			{
				return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
			}
		}
	}
}
