using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Shared.Procedural;
using System.Data;

namespace Pobytne.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class LicenseController : ControllerBase
	{
		private LicenseService _licenseService;
		public LicenseController(LicenseService licenseService)
		{
			_licenseService = licenseService;
		}
		[HttpGet]
		//[Authorize(Roles = "Administrator")]
		public ActionResult<List<License>> Get()
		{//TODO: Get by module
			return _licenseService.GetLicenses().Result;
		}
    }
}
