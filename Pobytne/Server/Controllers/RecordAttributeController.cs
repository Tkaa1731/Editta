using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Controllers
{
	[Route("RecordAttribute")]
	[Authorize]
	[ApiController]
	public class RecordAttributeController(RecordService service) : Controller
	{
		private readonly RecordService _service = service;
		public const EPermition permition = EPermition.PropertiesRecord;

		[HttpGet]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		public async Task<IEnumerable<RecordAttribute>> Get([FromQuery] int moduleId)
		{
			return await _service.GetAttibutesByModule(moduleId);
		}
	}
}
