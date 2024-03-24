using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using System.Net;

namespace Pobytne.Server.Controllers
{

    [Route("License")]
	[ApiController]
    [Authorize]
	public class LicenseController(LicenseService licenseService) : ControllerBase
	{
		private readonly LicenseService _licenseService = licenseService;
		public const EPermition permition = EPermition.License;

		[HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
		public async Task<IActionResult> Get([FromQuery] string filterJSON = "")
		{
            try
            {
			    var licenses = await _licenseService.GetLicenses();
                return Ok(licenses);
            }
            catch(Exception ex)
            { 
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
		}
		[HttpPut]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
		public async Task<IActionResult> Update([FromBody] License updateLicense)
        {
            if (updateLicense is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var updatedLicense = await _licenseService.Update(updateLicense);
                if (updatedLicense is not null)
                    return Ok(updatedLicense);

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se uložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Insert([FromBody] License insertLicense)
        {
            if (insertLicense is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var insertedLicense = await _licenseService.Insert(insertLicense);
                if (insertedLicense is not null)
                    return Ok(insertedLicense);

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
                var result = await _licenseService.Delete(id);
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
