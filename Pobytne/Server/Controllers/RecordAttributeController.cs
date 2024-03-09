using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using System.Net;

namespace Pobytne.Server.Controllers
{
	[Route("RecordAttribute")]
	[Authorize]
	[ApiController]
	public class RecordAttributeController(RecordService service) : Controller
	{
		private readonly RecordService _service = service;
		public const EPermition permition = EPermition.RecordAttribute;

		[HttpGet]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		public async Task<IEnumerable<RecordAttribute>> Get([FromQuery] int moduleId)
		{
			return await _service.GetAttibutesByModule(moduleId);
		}
		[HttpPost]
		[PermissionAuthorize(permition, EAccess.FullAccess)]
		[Route("Insert")]
		public async Task<IActionResult> Insert([FromBody] RecordAttribute insertAttribute)
		{
			if (insertAttribute is null)
			{
				return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "No data to insert"));
			}
			try
			{
				await _service.Insert(insertAttribute);
				return Ok();
			}
			catch (Exception ex)
			{
				return Conflict(new ErrorResponse(HttpStatusCode.Conflict, ex.Message));
			}
		}
		[HttpPost]
		[PermissionAuthorize(permition, EAccess.FullAccess)]
		[Route("Update")]
		public async Task<IActionResult> Update([FromBody] RecordAttribute updateAttribute)
		{
			if (updateAttribute is null)
			{
				return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "No data to insert"));
			}
			try
			{
				await _service.Update(updateAttribute);
				return Ok();
			}
			catch (Exception ex)
			{
				return Conflict(new ErrorResponse(HttpStatusCode.Conflict, ex.Message));
			}
		}
	}
}
