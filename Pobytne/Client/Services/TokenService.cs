using Blazored.LocalStorage;
using Pobytne.Shared.Authentication;

namespace Pobytne.Client.Services
{
    public class TokenService
    {
        private readonly ILocalStorageService _localStorageService;
        public TokenService(ILocalStorageService localStorageService) => _localStorageService = localStorageService;

        public async Task<string?> GetToken() // pri vraceni do hlavicky
        {
            var result = string.Empty;
            try
            {
                var user = await _localStorageService.ReadEncryptedItem<UserAccount>(LocalStorageService.USER_SESSION);
                if (user != null)
                    result = user.Token;
            }
            catch
            {

            }
            return result;
        }
    }
}//TODO:DELETE