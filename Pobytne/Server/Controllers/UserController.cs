using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural;


namespace Pobytne.Server.Controllers
{
	[Route("User")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
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

        [HttpGet]
        [Route("UsersList")]
        public ActionResult<IEnumerable<User>> GetByLicOrMod([FromQuery]int licenseNumber = -1, [FromQuery]int userOfModule = -1)
        {
            if(licenseNumber > 0)
                return _userService.GetUsersByLicense(licenseNumber).Result.ToList();
            if(userOfModule > 0)
                return _userService.GetUsersByModule(userOfModule).Result.ToList();
            else
                return new List<User>();
        }

        [HttpGet]
        [Route("GetRest")]//GetRest?moduleId={module}"
        public ActionResult<IEnumerable<User>> GetRest([FromQuery] int moduleId)
        {
            return _userService.GetUsersExsModule(moduleId).Result.ToList();
        }

        [HttpGet]
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
        // DELETE api/<ItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        // POST api/<ItemsController>
        [HttpPost]
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
    }
}
