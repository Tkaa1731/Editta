using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
    [Serializable]
    [Table("S_Licence")]
    public class License : IContact, ICreation, IListItem
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int LicenseNumber { get; set; }// Insert Update.. same as ICO
        [Required(ErrorMessage ="Enter ICO of organization")]
        [Range(1000_0000,9999_9999,ErrorMessage ="Enter eight grade number")]
        public int ICO { get; set; }// Insert Update
        [Required(ErrorMessage ="Enter name of organization")]
        public string NameOfOrganization { get; set; } = string.Empty;// Insert Update
        [Required(ErrorMessage ="Enter contact person")]
        public string ContactPerson { get; set; } = string.Empty;// Insert Update
        [Required(ErrorMessage ="Enter type of version")]
        public int VersionType { get; set; }// Insert Update
        public bool IsDemo { get; set; }// Insert Update
        public bool IsPayed { get; set; }// Insert Update
        public bool IsBlocked { get; set; }// Insert Update
        public DateTime DateOfLaunch { get; set; } = DateTime.Now;
        public DateTime DateOfPayment { get; set; } = DateTime.MinValue;
        [Required(ErrorMessage ="Enter city")]
        public string City { get; set; } = string.Empty;// Insert Update
        [Required(ErrorMessage ="Enter street")]
        public string Street { get; set; } = string.Empty;// Insert Update
        [Required(ErrorMessage = "Enter post number")]
        public string PostNumber { get; set; } = string.Empty;// Insert Update
        [Required(ErrorMessage = "Enter email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;// Insert Update
        [Required(ErrorMessage = "Enter phone number")]
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;// Insert Update
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        public DateTime ValidTo { get; set; } = DateTime.Now.AddYears(1);// Insert Update
        public int CreationUserId { get; set; } // nastvajuji pred requestem
        public DateTime CreationDate { get; set; } // nastvajuji pred requestem
        [Editable(false)]
        public string CreationUserName { get; set; } = string.Empty;
        [Editable(false)]
        public string Name => NameOfOrganization;
        [Editable(false)]
        public string Description => ICO.ToString(); 
    }
}
