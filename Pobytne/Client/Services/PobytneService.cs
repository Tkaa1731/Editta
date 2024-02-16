using Blazored.LocalStorage;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pobytne.Shared.Authentication;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Pobytne.Client.Services
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class PobytneService
    {

        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _storageService;
        private static string GetControler(Type obj)
        {
            return obj.Name;
        }
        private async Task UpdateHeader(int? ModuleId = null)
        {
            var service = new AuthenticationService(_storageService,this, sp);
            var token = await service.GetToken();
            if(!token.IsNullOrEmpty())
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            if(ModuleId is not null)
            {
                if (_httpClient.DefaultRequestHeaders.Contains("X-module-id"))
                    _httpClient.DefaultRequestHeaders.Remove("X-module-id");// pokud existuje tak ji odstranim

                _httpClient.DefaultRequestHeaders.Add("X-module-id", ModuleId.ToString());
            }
        }

		private IServiceProvider sp;
		public PobytneService(HttpClient httpClient, ILocalStorageService localStorage, IServiceProvider sp)
		{
			_httpClient = httpClient;
			_storageService = localStorage;
			this.sp = sp;
		}
		//Allow UnAuthorized
		public async Task<object?> LoginAsync(LoginRequest obj)
        {
            var response = await _httpClient.PostAsJsonAsync($"/Auth/Login", obj);

            var responseStatusCode = response.StatusCode;

            if (responseStatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserAccount>(responseBody);
            }
            //else
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)responseStatusCode,
                ErrorMessage = "HTTP request failed with status code " + responseStatusCode
            };
            return errorResponse;
        }
		//Allow UnAuthorized
		public async Task<object?> RevokeAsync(RefreshRequest obj)
        {
            var response = await _httpClient.DeleteAsync($"/Auth/Revoke?id={obj.UserId}");

            var responseStatusCode = response.StatusCode;

            if (responseStatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            //else
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)responseStatusCode,
                ErrorMessage = "HTTP request failed with status code " + responseStatusCode
            };
            return errorResponse;
        }
		//Allow UnAuthorized
		public async Task<object?> RefreshAsync(RefreshRequest obj)
        {
            var response = await _httpClient.PostAsJsonAsync($"/Auth/Refresh", obj);

            var responseStatusCode = response.StatusCode;

            if (responseStatusCode == HttpStatusCode.OK)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UserAccount>(responseBody);
            }
            //else
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)responseStatusCode,
                ErrorMessage = "HTTP request failed with status code " + responseStatusCode
            };
            return errorResponse;
        }

        public async Task<object?> GetAllAsync<T>(string requestUri, int ModuleId)
        {
            var request = $"/{GetControler(typeof(T))}/{requestUri}";
			var requestMessage = new HttpRequestMessage(HttpMethod.Get, request);

            await UpdateHeader(ModuleId);

            return await SendAsyncAuthorizedHandler<IEnumerable<T>>(requestMessage);
		}

        public async Task<object?> GetByIdAsync<T>(int Id, int ModuleId)      
        {
            var request = $"/{GetControler(typeof(T))}/{Id}";
			var requestMessage = new HttpRequestMessage(HttpMethod.Get, request);

            await UpdateHeader(ModuleId);

            return await SendAsyncAuthorizedHandler<T>(requestMessage);
		}
        public async Task<object?> GetCountAsync<T>(string requestUri, int ModuleId)      
        {
            var request = $"/{GetControler(typeof(T))}/{requestUri}";
            var requestMessage = new HttpRequestMessage (HttpMethod.Get, request);

            await UpdateHeader(ModuleId);

            return await SendAsyncAuthorizedHandler<int>(requestMessage);
		}
		public async Task<object?> GetFilteredReports<T,P>(T obj, int ModuleId)
		{
			var request = $"/{GetControler(typeof(P))}/Filtered";

			await UpdateHeader(ModuleId);

			return await PostFilterAsyncAuthorizedHandler<T,P>(request, obj);
		}
		public async Task<object?> InsertAsync<T>(T obj, int ModuleId)
        {
            var request = $"/{GetControler(typeof(T))}/Insert";

            await UpdateHeader(ModuleId);

            return await PostAsyncAuthorizedHandler(request, obj);
        }
        public async Task<object?> UpdateAsync<T>(T obj, int ModuleId)
        {
            var request = $"/{GetControler(typeof(T))}/Update";

            await UpdateHeader(ModuleId);

            return await PostAsyncAuthorizedHandler(request, obj);
        }

        public async Task<object?> DeleteAsync<T>(int Id, int ModuleId)
        {
            var request = $"/{GetControler(typeof(T))}/{Id}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, request);

            await UpdateHeader(ModuleId);

            return await SendAsyncAuthorizedHandler<bool>(requestMessage);
		}
		// Private Authenticitation Handlers
		private async Task<object?> PostAsyncAuthorizedHandler<T>(string request, T obj)
		{
			var response = await _httpClient.PostAsJsonAsync(request, obj);

			var responseStatusCode = response.StatusCode;
			var message = response.Headers.WwwAuthenticate.ToString();

			if (responseStatusCode == HttpStatusCode.OK)
				return Task.CompletedTask;
			else if (responseStatusCode == HttpStatusCode.Unauthorized && message.Contains("invalid_token") && message.Contains("The access token expired"))
			{
				var auth = new AuthenticationService(_storageService, this, sp);
				if (await auth.Refresh())
				{
					await UpdateHeader();
					return await PostAsyncAuthorizedHandler(request, obj);// Pokud se podari refresh posli dotaz znovu
				}
			}

			var errorResponse = new ErrorResponse
			{
				StatusCode = (int)responseStatusCode,
				ErrorMessage = "HTTP request failed with status code " + responseStatusCode
			};
			return errorResponse;
		}
		private async Task<object?> PostFilterAsyncAuthorizedHandler<T,P>(string request, T obj)
		{
			var response = await _httpClient.PostAsJsonAsync(request, obj);

			var responseStatusCode = response.StatusCode;
			var message = response.Headers.WwwAuthenticate.ToString();

			if (responseStatusCode == HttpStatusCode.OK)
            {
				var responseBody = await response.Content.ReadAsStringAsync();
			    return JsonConvert.DeserializeObject<IEnumerable<P>>(responseBody);
            }
			else if (responseStatusCode == HttpStatusCode.Unauthorized && message.Contains("invalid_token") && message.Contains("The access token expired"))
			{
				var auth = new AuthenticationService(_storageService, this, sp);
				if (await auth.Refresh())
				{
					await UpdateHeader();
					return await PostFilterAsyncAuthorizedHandler<T,P>(request, obj);// Pokud se podari refresh posli dotaz znovu
				}
			}

			var errorResponse = new ErrorResponse
			{
				StatusCode = (int)responseStatusCode,
				ErrorMessage = "HTTP request failed with status code " + responseStatusCode
			};
			return errorResponse;
		}
		private async Task<object?> SendAsyncAuthorizedHandler<T>(HttpRequestMessage requestMessage)
		{
            var response = await _httpClient.SendAsync(requestMessage);

			var responseStatusCode = response.StatusCode;
			var message = response.Headers.WwwAuthenticate.ToString();

			if (responseStatusCode == HttpStatusCode.OK)
			{
				var responseBody = await response.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(responseBody);
			}
			else if (responseStatusCode == HttpStatusCode.Unauthorized && message.Contains("invalid_token") && message.Contains("The token expired"))
			{
				var auth = new AuthenticationService(_storageService, this, sp);
				if (await auth.Refresh())
                {
                    var newRequest = new HttpRequestMessage(requestMessage.Method, requestMessage.RequestUri);
                    await UpdateHeader();
					return await SendAsyncAuthorizedHandler<T>(newRequest);// Pokud se podari refresh posli dotaz znovu
                }
			}

			var errorResponse = new ErrorResponse
			{
				StatusCode = (int)responseStatusCode,
				ErrorMessage = "HTTP request failed with status code " + responseStatusCode
			};
			return errorResponse;
		}

	}
}
