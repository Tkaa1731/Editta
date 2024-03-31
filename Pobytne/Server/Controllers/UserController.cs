using AuthRequirementsData.Authorization;
using MailKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using System.Net;
using MailService = Pobytne.Server.Service.MailService;


namespace Pobytne.Server.Controllers
{
    [Route("User")]
    [ApiController]
    [Authorize]
    public class UserController(UserService userService, AuthService authService, MailService mailService) : ControllerBase
    {
        private readonly UserService _userService = userService;
        private readonly MailService _mailService = mailService;
        private readonly AuthService _authService = authService;
        private const EPermition permition = EPermition.LoginUser;

        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        public async Task<IActionResult> Get([FromQuery]int licenseNumber = -1, [FromQuery] int moduleId = -1, [FromQuery] string filterJSON = "")
        {
            try
            {
                if(licenseNumber > 0)
                {
                    var users =  await _userService.GetUsersByLicense(licenseNumber);
                    return Ok(users);
                }
                if (moduleId > 0)
                {
                    var users = await _userService.GetUsersExsModule(moduleId);
                    return Ok(users);
                }
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest,"Vyskytla se chyba v dotazu."));
            }
            catch(Exception ex) 
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("Count")]
        public async Task<IActionResult> GetCount([FromBody] LazyList lazyValues, [FromQuery] int licenseNumber)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{id}")]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetUserById(id);
                if (user is not null)
                    return Ok(user);
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, $"Uživatel ID: {id} není v databázi."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPut]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Update([FromBody]User updateUser) 
        {
            if (updateUser is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var updatedUser = await _userService.Update(updateUser);
                if (updatedUser is not null)
                    return Ok(updatedUser);

                return BadRequest(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se uložit záznam."));

            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPut]
        [Route("ForgotPasswordEmail")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPasswordEmail([FromBody] User resetUser)
        {
            if (resetUser is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var updateUser = await _userService.GetUserByEmail(resetUser.Email);

                var token = _authService.GetPasswordChageToken(updateUser);
                await _mailService.SendRestorePasswordMail(new(updateUser.Email, updateUser.Name, token) { Headline = "Žádost o změnu hesla" });

                return NoContent();
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        public async Task<IActionResult> Insert([FromBody] User insertUser)
        {
            if (insertUser is null)
            {
                return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest, "Vyskytla se chyba v dotazu."));
            }
            try
            {
                var insertedUser = await _userService.Insert(insertUser);
                if (insertedUser is null)
                    return BadRequest(new ErrorResponse(HttpStatusCode.BadRequest,"Nepovedlo se vložit záznam."));

                var token = _authService.GetPasswordChageToken(insertedUser);
                await _mailService.SendRestorePasswordMail(new(insertedUser.Email, insertedUser.Name, token) { Headline = "Byl Vám vytvořen účet" });

                return Ok(insertedUser);
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
