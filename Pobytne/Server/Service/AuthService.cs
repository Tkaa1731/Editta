using Irony.Parsing;
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
    public class AuthService(UserService userService, IMemoryCache memoryCache, IConfiguration configuration)
    {
		private readonly UserService _userService = userService;
		private readonly IConfiguration _configuration = configuration;
		private readonly IMemoryCache _memoryCache = memoryCache;

		private SymmetricSecurityKey Key => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:SecretKey").Value!));

        public async Task<UserAccount> Login(LoginRequest request)
		{
			var userAccount = await _userService.GetAccount(request);
			
			userAccount.Refresh = GenerateRefreshToken();
			StoreRefreshToken(userAccount.User.Id, userAccount.Refresh);
			GenerateJwtToken(userAccount);

			var urlSafe = EncodeUrlSafeBase64(userAccount.Token);

			return userAccount;
		}
		public async Task<UserAccount> Refresh(RefreshRequest request)
		{
			if (!_memoryCache.TryGetValue(request.UserId, out string? refreshToken) || refreshToken != request.RefreshToken)
				throw new Exception("Daný JWT nelze aktualizovat");

			var user = await _userService.GetUserById(request.UserId) ?? throw new Exception("Reference na neexistujiciho uzivatele");

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
		public async Task ResetPassword(PasswordRequest request)
		{
			if (request.Password != request.PasswordConfirm)
				throw new Exception("Hesla se neshoduji");

			var token = DecodeUrlSafeBase64(request.JWT);
			var claims = ReadJwtToken(token);

			var userId = claims.FindFirst(c => c.Type == ClaimTypes.Sid) ?? throw new Exception("Chybny JWT token");
            int UserId = int.Parse(userId.Value);

			var user = await _userService.GetUserById(UserId) ?? throw new Exception("Reference na neexistujiciho uzivatele");
			user.Password = request.Password;

			_ = await _userService.Update(user) ?? throw new Exception("Chyba pri aktualizaci hesla");
        }
		public string GetPasswordChageToken(User user)
		{
			var token = GenerateJwtToken(user.Id);
			return EncodeUrlSafeBase64(token);
		}
		private string GenerateRefreshToken()
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
		private ClaimsPrincipal ReadJwtToken(string token)
		{
            try
            {
				var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
				var validationParameters = new TokenValidationParameters
				{
					ValidateIssuerSigningKey = true,
					IssuerSigningKey = Key,
					ValidateIssuer = false, // Zde můžete změnit na true, pokud chcete ověřovat vydavatele.
					ValidateAudience = false, // Zde můžete změnit na true, pokud chcete ověřovat posluchače.
					ValidateLifetime = true
				};
				ClaimsPrincipal claimsPrincipal = jwtSecurityTokenHandler.ValidateToken(token, validationParameters, out _);
				return claimsPrincipal;
			}
			catch (Exception)
            {
				throw new Exception("Platnost odkazu vypršela");
			}

        }
        private void GenerateJwtToken(UserAccount userAccount)
		{
			var expireMinutes = _configuration.GetSection("JWT:ExpireMin").Value!;
			var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(int.Parse(expireMinutes));

			User user = userAccount.User;
			var claimList = new List<Claim>
				{
					new(ClaimTypes.Sid,user.Id.ToString()),
					new(ClaimTypes.Name,user.UserLogin),
					new(ClaimTypes.Email,user.Email),
					new(ClaimTypes.GivenName,user.UserName),
					new("License",user.LicenseNumber.ToString()),
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
				Key,
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
        private string GenerateJwtToken(int userId)
        {
            var expireMinutes = _configuration.GetSection("JWT:ExpireMin").Value!;
            var tokenExpiryTimeStamp = DateTime.UtcNow.AddMinutes(int.Parse(expireMinutes));

            var claimList = new List<Claim>
                {
                    new(ClaimTypes.Sid,userId.ToString())
                };

            var claimsIdentity = new ClaimsIdentity(claimList, "JwtAuth");

            var signingCredentials = new SigningCredentials( //klic a algoritmus sifry
                Key,
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

            return token;
        }
        private string EncodeUrlSafeBase64(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            string base64 = Convert.ToBase64String(bytes);
            string urlSafeBase64 = base64.Replace('+', '-').Replace('/', '_').Replace("=", "");
            return urlSafeBase64;
        }
        private string DecodeUrlSafeBase64(string input)
        {
            string urlSafeBase64 = input.Replace('-', '+').Replace('_', '/');
            switch (urlSafeBase64.Length % 4)
            {
                case 2: urlSafeBase64 += "=="; break;
                case 3: urlSafeBase64 += "="; break;
            }
            byte[] bytes = Convert.FromBase64String(urlSafeBase64);
            string original = Encoding.UTF8.GetString(bytes);
            return original;
        }
    }
}
