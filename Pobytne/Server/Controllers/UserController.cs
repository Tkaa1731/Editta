using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pobytne.Server.Service;
using Pobytne.Server.Authentication;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Pobytne.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public ActionResult<UserAccount> LoginUser([FromBody]LoginRequest request)
        {
            UserAccount user;
            try
            {
                var jwtAuthenticationManager = new JwtAuthenticationManager(_userService);
                user = jwtAuthenticationManager.GenerateJwtToken(request.Name, request.Password);
                return user;
            }
            catch {
                return Unauthorized();
            }

            //User loginedUser = _userService.LoginUser(user).Result;
            //if (loginedUser == null) { return null; }
            //return loginedUser;
        }
        [HttpPost]
        [Route("Update")]
        public ActionResult<User?> Update([FromBody]User updateUser) 
        {
            return _userService.Update(updateUser).Result;
        }
        [HttpGet]
        [Route("UsersList")]
        //[Authorize(Roles = "Administrator")]
        public ActionResult<IEnumerable<User>> Get([FromQuery]int licenseNumber = -1, [FromQuery]int userOfModule = -1)
        {//TODO: Get by module
            //long licenseNumber = 26591537;
            if(licenseNumber > 0)
                return _userService.GetUsersByLicense(licenseNumber).Result.ToList();
            if(userOfModule > 0)
                return _userService.GetUsersByModule(userOfModule).Result.ToList();
            else
                return new List<User>();
        }
    }
}
