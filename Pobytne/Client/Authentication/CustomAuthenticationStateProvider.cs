using Microsoft.AspNetCore.Components.Authorization;
using Pobytne.Client.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Pobytne.Client.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly AuthenticationService _service;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public CustomAuthenticationStateProvider(AuthenticationService service)
        {
            _service = service;
        }

        private static List<Claim> MapClaims(IEnumerable<Claim> original)
        {
            var mappedClaims = new List<Claim>();

            foreach (var claim in original)
            {
                switch (claim.Type)
                {
                    case "unique_name":
                        mappedClaims.Add(new Claim(ClaimTypes.Name, claim.Value));
                        break;
                    case "email":
                        mappedClaims.Add(new Claim(ClaimTypes.Email, claim.Value));
                        break;
                    case "given_name":
                        mappedClaims.Add(new Claim(ClaimTypes.GivenName, claim.Value));
                        break;
                    default:
                        mappedClaims.Add(claim); // Pokud se nerozpozná, ponechá se původní Claim
                        break;
                }
            }

            return mappedClaims;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {

            var user = await _service.GetValidUser();// pri kaydem dotazu na authentication State je kontrola expirace + pripadny refresh
            if (user == null)
                return new AuthenticationState(_anonymous);// neni nic v Local storage
            
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var identity = new ClaimsIdentity();
            
            if (tokenHandler.CanReadToken(user.Token))
            {
                var jwtSecurityToken = tokenHandler.ReadJwtToken(user.Token);
                identity = new ClaimsIdentity(MapClaims(jwtSecurityToken.Claims), "JwtAuth");
            }
            var claimsPrincipal = new ClaimsPrincipal(identity);
            
            return new AuthenticationState(claimsPrincipal);
 
        }
        public void UpdateAuthenticationState()
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
