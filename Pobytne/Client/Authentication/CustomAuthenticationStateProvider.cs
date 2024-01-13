using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Pobytne.Client.Services;
using Pobytne.Shared.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Pobytne.Client.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService) => _localStorageService = localStorageService;

        private IEnumerable<Claim> MapClaims(IEnumerable<Claim> original)
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
            try
            {
                var user = await _localStorageService.ReadEncryptedItem<UserAccount>(LocalStorageService.USER_SESSION);// get entity of UserAccount from LS
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
                var authenticationState = new AuthenticationState(claimsPrincipal);

                return await Task.FromResult(authenticationState);
            }
            catch
            {
                return await Task.FromResult(new AuthenticationState(_anonymous));
            }
        }
        public async Task UpdateAuthenticationState(UserAccount? user)
        {
            ClaimsPrincipal claimsPrincipal;
            if (user != null)
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var identity = new ClaimsIdentity();

                if (tokenHandler.CanReadToken(user.Token))
                {
                    var jwtSecurityToken = tokenHandler.ReadJwtToken(user.Token);
                    identity = new ClaimsIdentity(MapClaims(jwtSecurityToken.Claims), "JwtAuth");
                }
                claimsPrincipal = new ClaimsPrincipal(identity);

                user.ExpiryTimeStamp = DateTime.Now.AddSeconds(user.ExpiresIn);
                await _localStorageService.SaveItemEncrypted(LocalStorageService.USER_SESSION, user);
            }
            else
            {
                claimsPrincipal = _anonymous;
                await _localStorageService.RemoveItemAsync(LocalStorageService.USER_SESSION);
            }
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
