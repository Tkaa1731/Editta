using Blazored.LocalStorage;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Procedural;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace Pobytne.Client.Authentication
{
    internal class AuthenticationService
    {
        private  readonly ILocalStorageService _storageService;
        private  readonly HttpClient _httpClient;

        public AuthenticationService(ILocalStorageService storageService, HttpClient httpClient)
        {
            _storageService = storageService;
            _httpClient = httpClient;
        }
        public  async Task Login(LoginRequest logRequest)
        {   
            var logResponse = await _httpClient.PostAsJsonAsync("/api/User/Login", logRequest);
            if (logResponse.IsSuccessStatusCode)
            {
                var user = await logResponse.Content.ReadFromJsonAsync<UserAccount>();
                var customAuthStateProvider = new CustomAuthenticationStateProvider(_storageService);
                await customAuthStateProvider.UpdateAuthenticationState(user);
            }
            else if (logResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("Wrong UserName or Password");
            }
        }
    }
}
