using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
    [Table("S_LoginUser")]
    public class User : ICreation
    {
        [Key]
        public int Id { get; set; }
        public string UserLogin { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        [Editable(false)]
        private string _password { get; set; } = string.Empty;
        public string Password { set { _password = value; } get { return _password; } }
        public bool PasswordIsInicial { get; set;} = false;
        public int LicenseNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        public int CustomerId { get; set; } = 0;
        public bool Valid { get; set; }
        [Editable(false)]
        public List<Permition>? AccessPermition { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        [Editable(false)]
        public string CreationUserName { get; set; } = string.Empty;

        public bool CheckPassword(string password) => _password == password;
    }
}
