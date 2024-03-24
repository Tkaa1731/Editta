using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Client.Extensions.IDirectory;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural;
using Pobytne.Shared.Struct;
using System.Net;

namespace Pobytne.Server.Controllers
{
    [Route("RecordAttribute")]
	[Authorize]
	[ApiController]
	public class RecordAttributeController(RecordAttributeService service) : Controller
	{
		private readonly RecordAttributeService _service = service;
		public const EPermition permition = EPermition.RecordAttribute;

		[HttpGet]
		[PermissionAuthorize(permition, EAccess.ReadOnly)]
		public async Task<IActionResult> Get([FromQuery] int moduleId, [FromQuery] string filterJSON = "")
		{
            try
            {
                var attributes = await _service.GetAttibutesByModule(moduleId);
                return Ok(attributes);
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
		}
		[HttpPut]
		[PermissionAuthorize(permition, EAccess.FullAccess)]
		public async Task<IActionResult> Update([FromBody] RecordAttribute updateAttribute)
		{
			if (updateAttribute is null)
			{
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
			{
				var updatedAttribute = await _service.Update(updateAttribute);
                if (updatedAttribute is not null)
                    return Ok(updatedAttribute);

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se uložit záznam."));
            }
			catch (Exception ex)
			{
				return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
			}
		}
		[HttpPost]
		[PermissionAuthorize(permition, EAccess.FullAccess)]
		public async Task<IActionResult> Insert([FromBody] RecordAttribute insertAttribute)
		{
			if (insertAttribute is null)
			{
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
			{
				var insertedAttribute = await _service.Insert(insertAttribute);
                if (insertedAttribute is not null)
                    return Ok(insertedAttribute);

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
