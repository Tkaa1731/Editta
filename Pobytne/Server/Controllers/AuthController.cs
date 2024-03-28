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
				return Ok(user);
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
            try
            {
                var user = await _authService.Refresh(request);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

		}
        [HttpPost]
		[AllowAnonymous]
		[Route("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordRequest request)
        {
			try
			{
				await _authService.RefreshPassword(request);
				return Ok();
			}
			catch(Exception ex) 
			{
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }

        }
        [HttpDelete]
        [Route("Revoke")]
        public IActionResult Revoke(int id)
        {
			try
			{
				_authService.RemoveRefreshToken(id);
				return NoContent();
			}
			catch(Exception ex)
			{
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));

            }
        }
    }
}
