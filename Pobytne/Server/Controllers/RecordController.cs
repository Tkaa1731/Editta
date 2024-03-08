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
    [Route("Record")]
    [Authorize]
    [ApiController]
    public class RecordController(RecordService service) : ControllerBase
	{
        private readonly RecordService _service = service;
		public const EPermition permition = EPermition.Record;

		[HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("RecordsBranch")]
        public async Task<IEnumerable<Record>> GetBranch(int parentId)
        {
            return await _service.GetBranch(parentId);
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

        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] Record updateRecord)
        {
            if (updateRecord is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest,"No data to update"));
            }
            try
            {
                await _service.Update(updateRecord);
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.Conflict, ex.Message)) ;
            }
        }
        // POST api/<ItemsController>
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Insert")]
        public async Task<IActionResult> Insert([FromBody] Record insertRecord)
        {
            if (insertRecord is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "No data to insert"));
            }
            try
            {
                await _service.Insert(insertRecord);
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.Conflict, ex.Message));
            }
        }
        // DELETE api/<ItemsController>/5
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _service.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.Conflict, ex.Message));
            }
        }
    }
}
