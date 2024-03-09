using AuthRequirementsData.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;


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
        [Route("UsersList")]
        public async Task<IEnumerable<User>> GetByLicense([FromQuery]int licenseNumber)
        {
            return await _userService.GetUsersByLicense(licenseNumber);
        }

        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("GetRest")]//GetRest?moduleId={module}"
        public ActionResult<IEnumerable<User>> GetRestOfModule([FromQuery] int moduleId)
        {
            return _userService.GetUsersExsModule(moduleId).Result.ToList();
        }

        [HttpGet]
        [PermissionAuthorize(permition, EAccess.ReadOnly)]
        [Route("{id}")]
        public ActionResult<User> Get(int id)
        {
            try
            {
                User? u =  _userService.GetUserById(id).Result;
                if (u is not null)
                    return u;
                else
                    return BadRequest("Insert failed");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error :{ex.Message}");
            }
        }
        [HttpPost]
        [PermissionAuthorize(permition, EAccess.FullAccess)]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody]User updateUser) 
        {
            if (updateUser is null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                int rowsAffected = await _userService.Update(updateUser);
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
        public async Task<IActionResult> Insert([FromBody] User insertUser)
        {
            if (insertUser is null)
            {
                return BadRequest("Invalid data");
            }
            try
            {
                int? rowsAffected = await _userService.Insert(insertUser);
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
        //[PermissionAuthorize(permition, EAccess.FullAccess)]
        //[HttpDelete("{id}")]
        //public async Task<bool> Delete(int id)
        //{
        //    return await _userService.Delete(id) > 0; 
        //}
    }
}
