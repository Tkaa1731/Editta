using Microsoft.Extensions.Options;
using Pobytne.Client.Services;
using System.Net.Http.Headers;

namespace Pobytne.Client.Authentication
{
    public class JwtTokenInterceptor : DelegatingHandler
    {
        private readonly TokenService _tokenService;

        public JwtTokenInterceptor(TokenService tokenService) //: base(new HttpClientHandler())
        {
           _tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //// Check if the token is available and not expired
            //if (!string.IsNullOrEmpty(_tokenService.GetToken().Result))// && !_tokenService.IsExpired
            //{
            string? token = await _tokenService.GetToken();
            string Token;
            if (token is null)
                Token = "";
            else
                Token = token;
                // Add the JWT token to the Authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", Token);
            //}

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
