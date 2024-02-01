using Blazored.LocalStorage;
using Pobytne.Client.Authentication;
using Pobytne.Shared.Authentication;

namespace Pobytne.Client.Services
{
    internal class AuthenticationService
    {
        private readonly ILocalStorageService _storageService;
        private readonly PobytneService _pobytneService;

        public AuthenticationService(ILocalStorageService storageService, PobytneService pobytneService)
        {
            _storageService = storageService;
            _pobytneService = pobytneService;
        }
        public async Task<Task> Login(LoginRequest logRequest)
        {
            var logResponse = await _pobytneService.LoginAsync(logRequest);
            if (logResponse is ErrorResponse)
            {
                return Task.FromResult(logResponse);
            }

            var user = logResponse as UserAccount;
            var customAuthStateProvider = new CustomAuthenticationStateProvider(_storageService);
            await customAuthStateProvider.UpdateAuthenticationState(user);

            return Task.CompletedTask;
        } // update Token
        public async Task<bool> Refresh()
        {
            var user = await _storageService.ReadEncryptedItem<UserAccount>(LocalStorageService.USER_SESSION);
            if (user is null)
                return false;
            var logResponse = await _pobytneService.RefreshAsync(new RefreshRequest() { UserId = user.User.Id, RefreshToken = user.Refresh });
            if (logResponse is UserAccount u)
            {
                var customAuthStateProvider = new CustomAuthenticationStateProvider(_storageService);
                await customAuthStateProvider.UpdateAuthenticationState(u);
                return true;
            }
            return false;
        }
        public async Task Logout()
        {
            var user = await _storageService.ReadEncryptedItem<UserAccount>(LocalStorageService.USER_SESSION);
            if (user is not null)
            {
                await _pobytneService.RevokeAsync(new RefreshRequest() { UserId = user.User.Id, RefreshToken = user.Refresh });

                var customAuthStateProvider = new CustomAuthenticationStateProvider(_storageService);
                await customAuthStateProvider.UpdateAuthenticationState(null);// provede i remove item
            }
        }
        public async Task<bool> Revoke()
        {
            var user = await _storageService.ReadEncryptedItem<UserAccount>(LocalStorageService.USER_SESSION);
            if (user is null)
                return false;

            var logResponse = await _pobytneService.RevokeAsync(new RefreshRequest() { UserId = user.User.Id, RefreshToken = user.Refresh });
            if (logResponse is bool r)
                return r;

            return false;
        }
        public async Task<string?> GetValidToken() // pri vraceni do hlavicky
        {
            var result = string.Empty;

            var user = await _storageService.ReadEncryptedItem<UserAccount>(LocalStorageService.USER_SESSION);
            if (user != null && user.ExpiryTimeStamp > DateTime.UtcNow)
                result = user.Token;// TODO: Ukladat do lokalStrorage vse nebo jenom tokeny... ukladat zvlast?

            return result;
        }
        public async Task CheckTokenValidity()
        {
            string? token = await GetValidToken();
            if(string.IsNullOrEmpty(token))
                await Refresh();
        }
    }
}
