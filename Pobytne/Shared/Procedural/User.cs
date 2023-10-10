using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
    [Table("S_LoginUser")]
    public class User : ICreation, IListItem
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter UserLogin")]
        public string UserLogin { get; set; } = string.Empty; // Insert, Update
        [Required(ErrorMessage = "Enter UserName")]
        public string UserName { get; set; } = string.Empty; // Insert, Update
        [Editable(false)]
        private string _password { get; set; } = string.Empty;
        public string Password { set { _password = value; } get { return _password; } }
        public bool PasswordIsInicial { get; set;} = false;
        public int LicenseNumber { get; set; }
        [EmailAddress]
        public string Email { get; set; } = string.Empty; // Insert, Update
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;// Insert, Update
        public int CustomerId { get; set; } = 0;// Insert, Update ... zatim neresim Customera !! pridat do form
        public bool Valid { get; set; } = true; // Insert, Update
        [Editable(false)]
        public List<Permition>? AccessPermition { get; set; }
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Enter a date of the end of validation")]
        public DateTime ValidTo { get; set; } = DateTime.Now.AddYears(1);// Insert, Update
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        [Editable(false)]
        public string CreationUserName { get; set; } = string.Empty;
        [Editable(false)]
        public string Name => UserLogin;
        [Editable(false)]
        public string Description => UserName;
        public bool CheckPassword(string password) => _password == password;
    }
}
