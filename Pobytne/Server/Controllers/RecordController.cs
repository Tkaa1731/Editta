using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Controllers
{
    [Route("Record")]
    [Authorize]
    [ApiController]
    public class RecordController : ControllerBase
	{
        private readonly RecordService _service;
		public const EPermition permition = EPermition.Record;

		public RecordController(RecordService service)
        {
            _service = service;
        }
        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("RecordsBranch")]
        public async Task<IEnumerable<Record>> GetBranch(int parentId)
        {
            return await _service.GetRecords(parentId);
        }

        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("RecordsRoot")]
        public async Task<IEnumerable<Record>> GetRoot(int moduleId)
        {
            return await _service.GetRoot(moduleId);
        }

        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("RecordsMaxDepth")]
        public async Task<int> GetMaxDepth(int moduleId)
        {
            return await _service.GetMaxDepth(moduleId);
        }
    }
}
