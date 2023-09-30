using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Pobytne.Client.Extensions;
using Pobytne.Shared.Authentication;
using System.Security.Claims;

namespace Pobytne.Client.Authentication
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());
        public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()//TODO:GetToken()
        {
            try
            {
                var user = await _localStorageService.ReadEncryptedItem<UserAccount>(LocalStorageService.USER_SESSION);// get entity of UserAccount from LS
                if (user == null)
                    return await Task.FromResult(new AuthenticationState(_anonymous));// no one is logged
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim("Id",user.User.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.User.UserName),
                    new Claim("License",user.User.LicenseNumber.ToString())
                },"JwtAuth"));
                return await Task.FromResult(new AuthenticationState(claimsPrincipal));
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
                claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
                {
                    new Claim("Id",user.User.Id.ToString()),
                    new Claim(ClaimTypes.Name,user.User.UserName),
                    new Claim("License",user.User.LicenseNumber.ToString())

                }));
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
        public async Task<string> GetToken()
        {
            var result = string.Empty;
            try
            {
                var user = await _localStorageService.ReadEncryptedItem<UserAccount>(LocalStorageService.USER_SESSION);
                if (user != null && DateTime.Now < user.ExpiryTimeStamp)
                    result = user.Token;
            }
            catch
            {

            }
            return result;
        }
    }
}
