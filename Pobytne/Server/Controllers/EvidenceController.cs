using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural.FilterReports;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Controllers
{
	[Route("Evidence")]
	[Authorize]
	[ApiController]
	public class EvidenceController : ControllerBase
	{
        private readonly InteractionService _interactionService;
		private const EPermition permition = EPermition.EvidenceSummary;
		public EvidenceController(InteractionService interactionService) => _interactionService = interactionService;

		[HttpPost]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		[Route("Filtered")]
		public Task<IEnumerable<Evidence>> GetFiltered(EvidenceFilter filter)
		{
			return _interactionService.GetFilteredReports(filter);
		}
	}
}
