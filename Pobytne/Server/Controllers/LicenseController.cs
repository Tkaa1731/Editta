﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Shared.Procedural;
using System.Data;

namespace Pobytne.Server.Controllers
{
	[Route("[controller]/[action]")]
	[ApiController]
	public class LicenseController : ControllerBase
	{
		private LicenseService _licenseService;
		public LicenseController(LicenseService licenseService)
		{
			_licenseService = licenseService;
		}
		[HttpGet]
		//TODO:[Authorize(Roles = "Administrator")]
		public ActionResult<IEnumerable<License>> Get()
		{//TODO: Get by module
			return _licenseService.GetLicenses().Result.ToList();
		}
    }
}
