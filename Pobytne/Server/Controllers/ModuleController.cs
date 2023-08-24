using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Authentication;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural;
using System.Data;

namespace Pobytne.Server.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ModuleController : ControllerBase
	{
		private ModuleService _moduleService;
		public ModuleController(ModuleService moduleService)
		{
			_moduleService = moduleService;
		}
		[HttpGet]
		[Route("ModulesList")]
		//[Authorize(Roles = "Administrator")]
		public ActionResult<List<Module>> Get([FromQuery]long licenseNumber)
		{//TODO: Get by module
			return _moduleService.GetModules(licenseNumber).Result;
		}

    }
}
