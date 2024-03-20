using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Extensions;
using System.Net;

namespace Pobytne.Server.Controllers
{
	[Route("Auth")]
	[ApiController]
	public class AuthController(AuthService authService) : ControllerBase
	{
		private readonly AuthService _authService = authService;

        [HttpPost]
		[AllowAnonymous]
		[Route("Login")]
		public async Task<IActionResult> Login([FromBody] LoginRequest request)
		{
			try
			{
				var user = await _authService.Login(request);
				if(user is not null)		
					return Ok(user);
				return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nastala chyba při přihlašování"));
			}
			catch (Exception ex)
			{
				return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError,ex.Message));
			}
		}
		[HttpPost]
		[AllowAnonymous]
		[Route("Refresh")]
		public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
		{
			var user = await _authService.Refresh(request);
			if (user is null)
				return Unauthorized();
			return Ok(user);
		}
        [HttpDelete]
        [Route("Revoke")]
        public IActionResult Revoke(int id)
        {
            _authService.RemoveRefreshToken(id);
			return NoContent();
        }
    }
}
