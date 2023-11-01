using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural;
using System.Data;

namespace Pobytne.Server.Controllers
{

  [Route("License")]
  [Authorize]
	[ApiController]
	public class LicenseController : ControllerBase
	{
		private LicenseService _licenseService;
		public LicenseController(LicenseService licenseService)
		{
			_licenseService = licenseService;
		}
		[HttpGet]
		public ActionResult<IEnumerable<License>> Get()
		{//TODO: Get by module
			return _licenseService.GetLicenses().Result.ToList();
		}
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] License updateLicense)
        {
            if (updateLicense is null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                int rowsAffected = await _licenseService.Update(updateLicense);
                if (rowsAffected > 0)
                    return Ok("Update successful");
                else
                    return BadRequest("Update failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :{ex.Message}");
            }
        }
        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        // POST api/<ItemsController>
        [HttpPost]
        [Route("Insert")]
        public async Task<IActionResult> Insert([FromBody] License insertLicense)
        {
            if (insertLicense is null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                int? rowsAffected = await _licenseService.Insert(insertLicense);
                if (rowsAffected > 0)
                    return Ok("Insert successful");
                else
                    return BadRequest("Insert failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :{ex.Message}");
            }
        }
    }
}
