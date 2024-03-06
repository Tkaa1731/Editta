using System.Net;

namespace Pobytne.Shared.Extensions
{
    public class ErrorResponse(HttpStatusCode code, string message)
    {
        public HttpStatusCode StatusCode { get; set; } = code;
        public string ErrorMessage { get; set; } = message;
    }

}
