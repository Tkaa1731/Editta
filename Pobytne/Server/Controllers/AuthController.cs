using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Authentication;

namespace Pobytne.Server.Controllers
{
	[Route("Auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly AuthService _authService;
		public AuthController(AuthService authService)
		{
			_authService = authService;
		}

		[HttpPost]
		[AllowAnonymous]
		[Route("Login")]
		public async Task<ActionResult<UserAccount>> Login([FromBody] LoginRequest request)
		{
			var user = await _authService.Login(request);
			if(user is null)		
				return Unauthorized();
			return user;
		}
		[HttpPost]
		[AllowAnonymous]
		[Route("Refresh")]
		public async Task<ActionResult<UserAccount>> Refresh([FromBody] RefreshRequest request)
		{
			var user = await _authService.Refresh(request);
			if (user is null)
				return Unauthorized();// TODO: vracet vzdy objekt? Response?
			return Ok(user);
		}
        [HttpDelete]
        [Route("Revoke")]
        public ActionResult Revoke(int id)
        {
            _authService.RemoveRefreshToken(id);
			return Ok();
        }
    }
}
