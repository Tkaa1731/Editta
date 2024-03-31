using Blazored.LocalStorage;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Pobytne.Client.Pages;
using Pobytne.Shared.Authentication;
using Pobytne.Shared.Extensions;
using Pobytne.Shared.Procedural.DTO;
using Pobytne.Shared.Struct;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Web;

namespace Pobytne.Client.Services
{
    public class PobytneService(HttpClient httpClient, ILocalStorageService localStorage, IServiceProvider serviceProvider)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILocalStorageService _storageService = localStorage;
		private readonly IServiceProvider serviceProvider = serviceProvider;
        private static string GetController(Type obj)
        {
            return obj.Name;
        }
        private async Task UpdateHeader(int? ModuleId = null)
        {
            var service = new AuthenticationService(_storageService,this, serviceProvider);
            var token = await service.GetToken();
            if (!token.IsNullOrEmpty())
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if (ModuleId is not null)
            {
                if (_httpClient.DefaultRequestHeaders.Contains("X-module-id"))
                    _httpClient.DefaultRequestHeaders.Remove("X-module-id");// pokud existuje tak ji odstranim

                _httpClient.DefaultRequestHeaders.Add("X-module-id", ModuleId.ToString());
            }
        }

        //Allow UnAuthorized
        public async Task<object?> LoginAsync(LoginRequest obj)
        {
            var response = await _httpClient.PostAsJsonAsync($"/Auth/Login", obj);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<UserAccount>(responseBody);
            if (responseStatusCode == HttpStatusCode.Conflict)
                return JsonConvert.DeserializeObject<ErrorResponse>(responseBody);

            return new ErrorResponse(HttpStatusCode.InternalServerError,responseBody);
        }
        //Allow UnAuthorized
        public async Task RevokeAsync(RefreshRequest obj)
        {
            await _httpClient.DeleteAsync($"/Auth/Revoke?id={obj.UserId}");
        }
        //Allow UnAuthorized
        public async Task<object?> RefreshAsync(RefreshRequest obj)
        {
            var response = await _httpClient.PostAsJsonAsync($"/Auth/Refresh", obj);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == HttpStatusCode.OK)
				return JsonConvert.DeserializeObject<UserAccount>(responseBody);
			if (responseStatusCode == HttpStatusCode.Conflict)
                return JsonConvert.DeserializeObject<ErrorResponse>(responseBody);

            return new ErrorResponse(HttpStatusCode.InternalServerError,responseBody);
        }
        //Allow UnAuthorized
        public async Task<object?> UpdatePasswordAsync(PasswordRequest obj)
        {
            var response = await _httpClient.PostAsJsonAsync($"/Auth/ResetPassword", obj);

            var responseStatusCode = response.StatusCode;
            var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == HttpStatusCode.OK)
                return Task.CompletedTask;
            else if (responseStatusCode == HttpStatusCode.Conflict)
                return JsonConvert.DeserializeObject<ErrorResponse>(responseBody);

            return new ErrorResponse(HttpStatusCode.InternalServerError,responseBody);
        }
        public async Task<object?> GetAllAsync<T>(string requestUri, int ModuleId, LazyList filter = default!)
        {
            if (filter is not null)
            {
                string filterJSON = JsonConvert.SerializeObject(filter);
                requestUri += $"filterJSON={HttpUtility.UrlEncode(filterJSON)}";
            }

            var request = $"/{GetController(filter?.Type ?? typeof(T))}/{requestUri}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, request);

            await UpdateHeader(ModuleId);

            return await SendAsyncAuthorizedHandler<IEnumerable<T>>(requestMessage);
        }
        public async Task<object?> GetByIdAsync<T>(int Id, int ModuleId)
        {
            var request = $"/{GetController(typeof(T))}/{Id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, request);

            await UpdateHeader(ModuleId);

            return await SendAsyncAuthorizedHandler<T>(requestMessage);
        }

        public async Task<object?> GetCountAsync<T>(string requestUri, int ModuleId, LazyList filter = default!)
        {
            if (filter is not null)
            {
                string filterJSON = JsonConvert.SerializeObject(filter);
                requestUri += $"filterJSON={HttpUtility.UrlEncode(filterJSON)}";
            }
            var request = $"/{GetController(filter?.Type ?? typeof(T))}/Count{requestUri}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, request);

            await UpdateHeader(ModuleId);

            return await SendAsyncAuthorizedHandler<int>(requestMessage);
        }
		public async Task<object?> InsertAsync<T>(T obj, int ModuleId, string url = "")
        {
            var request = $"/{GetController(typeof(T))}/{url}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, request);

            var jsonContent = JsonConvert.SerializeObject(obj);
            requestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            await UpdateHeader(ModuleId);

            return await SendAsyncAuthorizedHandler<T>(requestMessage);
        }
        public async Task<object?> UpdateAsync<T>(T obj, int ModuleId, string url = "")
        {
            var request = $"/{GetController(typeof(T))}/{url}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Put, request);

            var jsonContent = JsonConvert.SerializeObject(obj);
            requestMessage.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            await UpdateHeader(ModuleId);

            return await SendAsyncAuthorizedHandler<T>(requestMessage);
        }

        public async Task<object?> DeleteAsync<T>(int Id, int ModuleId,Type Controller = default!)
        {
            var controller = GetController(Controller ?? typeof(T));
            var request = $"/{controller}/{Id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, request);

            await UpdateHeader(ModuleId);

            return await SendAsyncAuthorizedHandler<bool>(requestMessage);
		}

		// Private Authenticitation Handlers
        private async Task<object?> SendAsyncAuthorizedHandler<T>(HttpRequestMessage requestMessage)
        {
            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            var message = response.Headers.WwwAuthenticate.ToString();
			var responseBody = await response.Content.ReadAsStringAsync();

            if (responseStatusCode == HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<T>(responseBody);
            else if (responseStatusCode == HttpStatusCode.NoContent)
                return Task.CompletedTask;
            else if (responseStatusCode == HttpStatusCode.Unauthorized && message.Contains("invalid_token") && message.Contains("The token expired"))
            {
                var auth = new AuthenticationService(_storageService, this, serviceProvider);
                if (await auth.Refresh())
                {
                    var newRequest = new HttpRequestMessage(requestMessage.Method, requestMessage.RequestUri);
                    await UpdateHeader();
                    return await SendAsyncAuthorizedHandler<T>(newRequest);// Pokud se podari refresh posli dotaz znovu
                }
				return new ErrorResponse(responseStatusCode, "Vypršela platnost Vašeho přihlášení.");
			}
            else if (responseStatusCode == HttpStatusCode.Forbidden)
                return new ErrorResponse(responseStatusCode,"Nemáte dostatečná oprávnění");
            else if (responseStatusCode == HttpStatusCode.Conflict || responseStatusCode == HttpStatusCode.BadRequest)
                return JsonConvert.DeserializeObject<ErrorResponse>(responseBody);

            return new ErrorResponse(responseStatusCode, responseBody);
        }

    }
}
