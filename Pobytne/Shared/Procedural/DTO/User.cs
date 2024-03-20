using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural.DTO
{
    [Serializable]
    [Table("S_LoginUser")]
    public class User : ACreation, IListItem
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vyplňte login")]
        public string UserLogin { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vyplňte jméno")]
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool PasswordIsInicial { get; set; } = false;
        public int LicenseNumber { get; set; }
        [EmailAddress(ErrorMessage ="Vyplňte korektní email")]
        public string Email { get; set; } = string.Empty;
        [Phone(ErrorMessage = "Vyplňte korektní telefonní číslo")]
        public string PhoneNumber { get; set; } = string.Empty;
        public int ClientId { get; set; } = 0;
        public bool Valid { get; set; } = true;
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidTo { get; set; } = DateTime.Now.AddYears(1);
        // Aplication propetries
        [Editable(false)]
        public List<Permition> AccessPermition { get; set; } = [];
        //IListItem
        [Editable(false)]
        public string Name => UserLogin;
        [Editable(false)]
        public string Description => UserName;
        [Editable(false)]
        public bool Active => Valid && ValidTo >= DateTime.Now;

        public bool CheckPassword(string password) => Password == password;

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
