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
        [HttpGet]
        [Route("UsersList")]
        //[Authorize(Roles = "Administrator")]
        public ActionResult<List<User>> Get([FromQuery]long licenseNumber)
        {//TODO: Get by module
            //long licenseNumber = 26591537;
            return _userService.GetUsers(licenseNumber).Result;
        }
        [HttpGet]
        [Route("List")]
        //[Authorize(Roles = "Administrator")]
        public ActionResult<List<User>> GetList()
        {//TODO: Get by module
            //long licenseNumber = 26591537;
            return new List<User>() { new Shared.Procedural.User { UserName="Terezka" } };
        }
    }
}
