using Microsoft.Extensions.Options;
using Pobytne.Client.Services;
using System.Net.Http.Headers;

namespace Pobytne.Client.Authentication
{
    internal class JwtTokenInterceptor : DelegatingHandler
    {
        /// <summary>
        /// private readonly AuthenticationService _service;
        /// </summary>
        private readonly TokenService _tokenService;

        public JwtTokenInterceptor(TokenService service) //: base(new HttpClientHandler()), AuthenticationService authenticationService
        {
            _tokenService = service;
            //_service = authenticationService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //// Check if the token is available and not expired
            //string? token = await _service.GetValidToken();
            //if (string.IsNullOrEmpty(token))
            //{// Do refresh
            //    if(!await _service.Refresh())// nezdari se refresj
            //        await _service.Logout();
            //}

            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            //return await base.SendAsync(request, cancellationToken);

            string? token = await _tokenService.GetToken();
            string Token;
            if (token is null)
                Token = "";
            else
                Token = token;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
