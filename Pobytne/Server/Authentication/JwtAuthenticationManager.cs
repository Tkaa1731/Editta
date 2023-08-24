using Microsoft.IdentityModel.Tokens;
using Pobytne.Server.Service;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Pobytne.Server.Authentication
{
    public class JwtAuthenticationManager
    {
        public const string JWT_SECURITY_KEY = "yPkCqn4kSWLtaJwXvN2jGzpQRyTZ3gdXkt7FeBJP";
        private const int JWT_TOKEN_VALIDITY_MINS = 20;

        private UserService _userService;

        public JwtAuthenticationManager(UserService userService)
        {
            _userService = userService;
        }

        public UserAccount GenerateJwtToken(string userName, string password)
        {
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                return null;
            Task<UserAccount> userAccount;
            try
            {/* Validating the User Credentials */
                userAccount = _userService.GetAccount(new LoginRequest { Name = userName, Password = password});            

                /* Generating JWT Token */
                var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
                var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);
                var claimsIdentity = new ClaimsIdentity(new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, userAccount.Result.UserName),
                        new Claim("License",userAccount.Result.LicenseNumber.ToString())
                    });
                var signingCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature);
                var securityTokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = claimsIdentity,
                    Expires = tokenExpiryTimeStamp,
                    SigningCredentials = signingCredentials
                };

                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
                var securityToken = jwtSecurityTokenHandler.CreateToken(securityTokenDescriptor);
                var token = jwtSecurityTokenHandler.WriteToken(securityToken);

                /* Returning the User Session object */
                var userSession = new UserAccount
                {
                    UserName = userAccount.Result.UserName,
                    AccessPermition = userAccount.Result.AccessPermition,
                    Token = token,
                    ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds
                };
                return userSession;
            }
            catch (Exception ex)
            {
                throw new Exception("Login error occurred: " + ex.Message);
            }
        }
    }

}
