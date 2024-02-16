using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Procedural.FilterReports;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Controllers
{
	[Route("CashRegister")]
	[Authorize]
	[ApiController]
	public class CashRegisterController : ControllerBase
	{
		private readonly InteractionService _interactionService;
		private const EPermition permition = EPermition.CashSummary;
		public CashRegisterController(InteractionService interactionService) => _interactionService = interactionService;

		[HttpPost]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		[Route("Filtered")]
		public Task<IEnumerable<CashRegister>> GetFiltered(CashRegisterFilter filter)
		{
			return _interactionService.GetFilteredReports(filter);
		}
	}
}
