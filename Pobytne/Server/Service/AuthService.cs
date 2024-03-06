using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using Pobytne.Client.Services;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Pobytne.Server.Service
{
    public class AuthService
	{
		private readonly UserService _userService;
		private readonly IConfiguration _configuration;
		private readonly IMemoryCache _memoryCache;
		public AuthService(UserService userService, IMemoryCache memoryCache, IConfiguration configuration)
		{
			_userService = userService;
			_memoryCache = memoryCache;
			_configuration = configuration;
		}
		public async Task<object> Login(LoginRequest request)
		{
			try
			{
				var userAccount = await _userService.GetAccount(request);

				userAccount.Refresh = GenerateRefreshToken();
				StoreRefreshToken(userAccount.User.Id, userAccount.Refresh);
				GenerateJwtToken(userAccount);
				
				return userAccount;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return new ErrorResponse(HttpStatusCode.Unauthorized, ex.Message);
			}
		}
		public async Task<UserAccount?> Refresh(RefreshRequest request)
		{
			if (_memoryCache.TryGetValue(request.UserId, out string? refreshToken) && refreshToken == request.RefreshToken)
			{
				try
				{
					var user = await _userService.GetUserById(request.UserId);

					// stejne jako login
					var userAccount = await _userService.GetAccount(
						new LoginRequest() { 
							Name = user.Name, 
							Password = user.Password }
						);

					userAccount.Refresh = GenerateRefreshToken();
					StoreRefreshToken(userAccount.User.Id, userAccount.Refresh);// Ulozi refresh token s klicem ID do cashe
					GenerateJwtToken(userAccount);// naplni instanci vytvorenym tokenem a lifetimem

					return userAccount;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
					return null;
				}
			}
			return null;
		}
		private static string GenerateRefreshToken()
		{
			byte[] randomNumber = new byte[32];
			using RandomNumberGenerator rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
		public void RemoveRefreshToken(int userId)
		{
			_memoryCache.Remove(userId);
		}
		private void StoreRefreshToken(int userId, string refreshToken)
		{
			var expire = _configuration.GetSection("Refresh:ExpireHours").Value!;
			var cacheEntryOptions = new MemoryCacheEntryOptions
			{
				AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(int.Parse(expire))
			};

			_memoryCache.Set(userId, refreshToken, cacheEntryOptions);
		}
		private void GenerateJwtToken(UserAccount userAccount)
		{
			var expireMinutes = _configuration.GetSection("JWT:ExpireMin").Value!;
			var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(int.Parse(expireMinutes));

			var tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				_configuration.GetSection("JWT:SecretKey").Value!));

			User user = userAccount.User;
			var claimList = new List<Claim>
				{
					new Claim(ClaimTypes.Sid,user.Id.ToString()),
					new Claim(ClaimTypes.Name,user.UserLogin),
					new Claim(ClaimTypes.Email,user.Email),
					new Claim(ClaimTypes.GivenName,user.UserName),
					new Claim("License",user.LicenseNumber.ToString()),
				};
			//add perminition strings
			foreach (var p in user.AccessPermition)
			{
				claimList.Add(
					new Claim(p.ModuleId.ToString(),p.PermitionString)
					);
			}

			var claimsIdentity = new ClaimsIdentity(claimList, "JwtAuth");

			var signingCredentials = new SigningCredentials( //klic a algoritmus sifry
				tokenKey,
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

			userAccount.Token = token;
			userAccount.ExpiryTimeStamp = tokenExpiryTimeStamp;// info pro klienta
		}
	}
}
