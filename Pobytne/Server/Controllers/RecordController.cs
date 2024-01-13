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
        public Task<IEnumerable<Record>> GetBranch(int parentId)
        {
            return _service.GetRecords(parentId);
        }
        [Authorize]
        [HttpGet]
        [Route("RecordsRoot")]
        public Task<IEnumerable<Record>> GetRoot(int moduleId)
        {
            return _service.GetRoot(moduleId);
        }
        [Authorize]
        [HttpGet]
        [Route("RecordsMaxDepth")]
        public Task<int> GetMaxDepth(int moduleId)
        {
            return _service.GetMaxDepth(moduleId);
        }
    }
}
