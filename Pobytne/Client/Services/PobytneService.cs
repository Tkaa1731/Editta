using Blazored.LocalStorage;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Pobytne.Shared.Authentication;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using static System.Net.WebRequestMethods;

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
        private static string GetControler(Type obj)
        {
            return obj.Name;
        }

        public PobytneService(HttpClient httpClient)
        {
            //httpClient.DefaultRequestHeaders.Add("Expires:", DateTime.Now.AddMinutes(1).ToString());

            _httpClient = httpClient;
        }
        public async Task<object?> LoginAsync(LoginRequest obj)
        {
            var response = await _httpClient.PostAsJsonAsync($"/User/Login", obj);

            var responseStatusCode = response.StatusCode;

            if (responseStatusCode.ToString() == "OK")
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
        public async Task<object?> DeleteAsync<T>(string requestUri, int Id)
        {
            var request = $"/{GetControler(typeof(T))}/{requestUri}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, request + Id);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            if (responseStatusCode.ToString() == "OK")
            {
                return await Task.FromResult(true);
            }
            //else
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)responseStatusCode,
                ErrorMessage = "HTTP request failed with status code " + responseStatusCode
            };
            return errorResponse;

        }

        public async Task<object?> GetAllAsync<T>(string requestUri)
        {
            var request = $"/{GetControler(typeof(T))}/{requestUri}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, request);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;

            if (responseStatusCode.ToString() == "OK")
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(responseBody);
            }
            //else
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)responseStatusCode,
                ErrorMessage = "HTTP request failed with status code " + responseStatusCode
            };
            return errorResponse;

        }

        public async Task<object?> GetByIdAsync<T>(int Id)
        {
            var request = $"/{GetControler(typeof(T))}/";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, request+Id);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;
            if (responseStatusCode.ToString() == "OK")
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(responseBody);
            }
            //else
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)responseStatusCode,
                ErrorMessage = "HTTP request failed with status code " + responseStatusCode
            };
            return errorResponse;
        }
        public async Task<object?> GetInfoAsync<T,R>(string requestUri)
        {
            var request = $"/{GetControler(typeof(T))}/{requestUri}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, request);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;

            if (responseStatusCode.ToString() == "OK")
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<R>(responseBody);
            }
            //else
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)responseStatusCode,
                ErrorMessage = "HTTP request failed with status code " + responseStatusCode
            };
            return errorResponse;
        }

        public async Task<object?> InsertAsync<T>(T obj)
        {
            var request = $"/{GetControler(typeof(T))}/Insert";

            var response = await _httpClient.PostAsJsonAsync(request, obj);

            var responseStatusCode = response.StatusCode;
            if (responseStatusCode.ToString() == "OK")         
                return Task.CompletedTask;
            //else
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)responseStatusCode,
                ErrorMessage = "HTTP request failed with status code " + responseStatusCode
            };
            return errorResponse;
        }

        public async Task<object?> UpdateAsync<T>(T obj)
        {
            var request = $"/{GetControler(typeof(T))}/Update";

            var response = await _httpClient.PostAsJsonAsync(request, obj);

            var responseStatusCode = response.StatusCode;
            if (responseStatusCode.ToString() == "OK")
                return Task.CompletedTask;
            //else
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)responseStatusCode,
                ErrorMessage = "HTTP request failed with status code " + responseStatusCode
            };
            return errorResponse;
        }
    }
}
