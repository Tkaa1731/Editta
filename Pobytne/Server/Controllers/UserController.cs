using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using System.Net;


namespace Pobytne.Server.Controllers
{
    [Route("User")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
		public const EPermition permition = EPermition.LoginUser;

		public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        public async Task<IActionResult> Get([FromQuery]int licenseNumber = -1, [FromQuery] int moduleId = -1)
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

                return NotFound(new ErrorResponse(HttpStatusCode.NotFound, "Nepovedlo se uložit záznam."));

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
                if (insertedUser is not null)
                    return Ok(insertedUser);
                
                return NotFound(new ErrorResponse(HttpStatusCode.NotFound,"Nepovedlo se vložit záznam."));
            }
            catch (Exception ex)
            {
                return Conflict(new ErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
}
