using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Procedural.FilterReports;
using Pobytne.Shared.Procedural.Filters;
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
        public async Task<IActionResult> Get([FromQuery] string filterJSON)
        {
            try
            {
                var filter = JsonConvert.DeserializeObject<RecordFilter>(filterJSON);
                if (filter is null)
                    return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));

                if (filter.ParentId > 0)
                {
                    var records = await _service.GetBranch(filter.ParentId);
                    return Ok(records);
                }
                if (filter.IsSeasonTicket)
                {
                    var ticket = await _service.GetSeasonTickets(filter.ModuleId,filter.ValidTo);
                    return Ok(ticket);
                }
                if(filter.ModuleId > 0)
                {
                    var records = await _service.GetRoot(filter.ModuleId); 
                    return Ok(records);
                }

                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("Count")]
        public async Task<IActionResult> GetMaxDepth([FromQuery] int moduleId)
        {
            try
            {
                var depth = await _service.GetMaxDepth(moduleId);
                return Ok(depth);
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpPut]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Update([FromBody] Record updateRecord)
        {
            if (updateRecord is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var updatedRecord = await _service.Update(updateRecord);
                if (updatedRecord is not null)
                    return Ok(updatedRecord);

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se uložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message)) ;
            }
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Insert([FromBody] Record insertRecord)
        {
            if (insertRecord is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var insertedRecord = await _service.Insert(insertRecord);
                if (insertedRecord is not null)
                    return Ok(insertedRecord);

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se vložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HttpDelete("{id}")]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _service.Delete(id);
                if (result > 0)
                    return NoContent();

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se smazat záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
