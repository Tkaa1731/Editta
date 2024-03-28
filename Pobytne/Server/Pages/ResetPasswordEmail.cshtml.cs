using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Pobytne.Server.Pages
{
    public class ResetPasswordEmailModel(string email, string name, string jwt) : PageModel
    {
        public string Email { get; } = email;
        public string Name { get; } = name;
        public string JWT {  get; } = jwt;
        public string Uri { get; set; } = "";
        public string Headline { get; set; } = "";
        public string JwtUri => Uri + "/Password/Reset/" + JWT;
        public void OnGet()
        {
        }
    }
}
