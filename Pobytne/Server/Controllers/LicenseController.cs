﻿using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;

namespace Pobytne.Server.Controllers
{

    [Route("License")]
	[ApiController]
    [Authorize]
	public class LicenseController : ControllerBase
	{
		private LicenseService _licenseService;
		public const EPermition permition = EPermition.License;

		public LicenseController(LicenseService licenseService)
		{
			_licenseService = licenseService;
		}
	    [PermissionAuthorize(permition, EAccess.ReadOnly)]
		[HttpGet]
		public async Task<IEnumerable<License>> Get()
		{//TODO: Get by module
			return await _licenseService.GetLicenses();
		}
		[HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Update")]
		public async Task<IActionResult> Update([FromBody] License updateLicense)
        {/////////////////
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
        // POST api/<ItemsController>
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
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
        // DELETE api/<ItemsController>/5
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
