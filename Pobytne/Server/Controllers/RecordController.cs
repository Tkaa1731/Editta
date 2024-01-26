using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural;

namespace Pobytne.Server.Controllers
{
    [Route("Record")]
    [ApiController]
    public class RecordController : Controller
    {
        private readonly RecordService _service;
        public RecordController(RecordService service)
        {
            _service = service;
        }
        [Authorize]
        [HttpGet]
        [Route("RecordsBranch")]
        public async Task<IEnumerable<Record>> GetBranch(int parentId)
        {
            return await _service.GetRecords(parentId);
        }
        [Authorize]
        [HttpGet]
        [Route("RecordsRoot")]
        public async Task<IEnumerable<Record>> GetRoot(int moduleId)
        {
            return await _service.GetRoot(moduleId);
        }
        [Authorize]
        [HttpGet]
        [Route("RecordsMaxDepth")]
        public async Task<int> GetMaxDepth(int moduleId)
        {
            return await _service.GetMaxDepth(moduleId);
        }
    }
}
