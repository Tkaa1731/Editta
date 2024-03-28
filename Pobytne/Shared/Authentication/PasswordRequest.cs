using System.ComponentModel.DataAnnotations;

namespace Pobytne.Shared.Authentication
{
    public class PasswordRequest
    {
        [Required(ErrorMessage = "Vyplňte nové heslo")]
        [RegularExpression(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,32}", ErrorMessage = "Heslo délky 8-32 znaků musí obsahovat malá, velká písmena, číslice a speciální znak")]
        public string Password { get; set; } = string.Empty;
        [Required(ErrorMessage = "Potvrďte heslo")]
        [Compare("Password", ErrorMessage = "Potvrďte odpovídající heslo")]
        public string PasswordConfirm { get; set; } = string.Empty;
        public string JWT { get; set; } = string.Empty;
    }
}
