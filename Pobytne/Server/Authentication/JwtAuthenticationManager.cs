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
		private const int JWT_TOKEN_VALIDITY_MINS = 30;

		private UserService _userService;

		public JwtAuthenticationManager(UserService userService)
		{
			_userService = userService;
		}
		//TODO: RefreshToken
		public UserAccount GenerateJwtToken(string userName, string password)
		{
			if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
				return null;
			Task<UserAccount> userAccount;
			try
			{/* Validating the User Credentials */
				userAccount = _userService.GetAccount(new LoginRequest { Name = userName, Password = password });
				User user = userAccount.Result.User;
				var userPermition = user.AccessPermition?.FirstOrDefault()?.PermitionString;
				if (userPermition == null)
					userPermition = "";

                /* Generating JWT Token */
                var tokenExpiryTimeStamp = DateTime.Now.AddMinutes(JWT_TOKEN_VALIDITY_MINS);
				var tokenKey = Encoding.ASCII.GetBytes(JWT_SECURITY_KEY);

				var claimList = new List<Claim>
					{
						new Claim(ClaimTypes.Sid,user.Id.ToString()),
						new Claim(ClaimTypes.Name,user.UserLogin),
						new Claim(ClaimTypes.Email,user.Email),
						new Claim(ClaimTypes.GivenName,user.UserName),
						new Claim("License",userAccount.Result.User.LicenseNumber.ToString()),
						new Claim("Permitions",userPermition)};

				var claimsIdentity = new ClaimsIdentity(claimList,"JwtAuth");

				var signingCredentials = new SigningCredentials( //klic a algoritmus sifry
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
				var userSession = userAccount.Result;
				userSession.Token = token;
				userSession.ExpiresIn = (int)tokenExpiryTimeStamp.Subtract(DateTime.Now).TotalSeconds;

				return userSession;
			}
			catch (Exception ex)
			{
				throw new Exception("Login error occurred: " + ex.Message);
			}
		}
	}

}