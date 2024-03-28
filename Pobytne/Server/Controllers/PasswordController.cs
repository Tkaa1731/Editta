using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using System.Net;

namespace Pobytne.Server.Controllers
{
    [Route("Password")]
    [ApiController]
    [Authorize]
    public class PasswordController(UserService userService, AuthService authService, MailService mailService) : ControllerBase
    {
        private readonly UserService _userService = userService;
        private readonly MailService _mailService = mailService;
        private readonly AuthService _authService = authService;

        public const EPermition permition = EPermition.LoginUser;

        [HttpPost]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        public async Task<IActionResult> UpdateAuthorize([FromBody] User updateUser)
        {
            if (updateUser is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var user = await _userService.UpdateRandomPassword(updateUser);
                if(user is null)
                    return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));

                var token = _authService.GetPasswordChageToken(user);
                await _mailService.SendRestorePasswordMail(new(user.Email, user.Name, token));

                _authService.RemoveRefreshToken(user.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPost]
        [Route("Anonym")]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateUnAuthorize([FromBody] string userEmail)
        {
            if (userEmail.IsNullOrEmpty())
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var updateUser = await _userService.GetUserByEmail(userEmail);
                if(updateUser is null)
                    return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
                
                var token = _authService.GetPasswordChageToken(updateUser);
                await _mailService.SendRestorePasswordMail(new(updateUser.Email, updateUser.Name, token) { Headline = "Žádost o změnu hesla "});

                return Ok();
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
