using Blazored.LocalStorage;
using Pobytne.Client.Authentication;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

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
    }
}
