using Newtonsoft.Json;
using Pobytne.Shared.Authentication;
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
        private static string GetControler(Type obj)
        {
            return obj.Name;
        }

        public PobytneService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<object?> LoginAsync(LoginRequest obj)
        {
            var response = await _httpClient.PostAsJsonAsync($"/Auth/Login", obj);

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
        public async Task<object?> RevokeAsync(RefreshRequest obj)
        {
            var response = await _httpClient.PostAsJsonAsync($"/Auth/Revoke", obj);

            var responseStatusCode = response.StatusCode;

            if (responseStatusCode.ToString() == "OK")
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(responseBody);
            }
            //else
            var errorResponse = new ErrorResponse
            {
                StatusCode = (int)responseStatusCode,
                ErrorMessage = "HTTP request failed with status code " + responseStatusCode
            };
            return errorResponse;
        }
        public async Task<object?> RefreshAsync(RefreshRequest obj)
        {
            var response = await _httpClient.PostAsJsonAsync($"/Auth/Refresh", obj);

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
        public async Task<object?> GetCountAsync<T>(string requestUri)
        {
            var request = $"/{GetControler(typeof(T))}/{requestUri}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, request);

            var response = await _httpClient.SendAsync(requestMessage);

            var responseStatusCode = response.StatusCode;

            if (responseStatusCode.ToString() == "OK")
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<int>(responseBody);
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
