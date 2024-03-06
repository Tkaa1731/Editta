using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlTypes;
using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural.DTO
{
    [Serializable]
    [Table("S_Licence")]
    public class License : ACreation, IListItem
    {
        [Key]
        public int Id { get; set; }
        public int LicenseNumber { get; set; }// InserCopy of ICO
        [Required(ErrorMessage = "Vyplňte IČO organizace")]
        [Range(1000_0000, 9999_9999, ErrorMessage = "Vyplňte osmimístné číslo")]
        public int ICO { get; set; }
        [Required(ErrorMessage = "Vyplňte název organizace")]
        public string NameOfOrganization { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vyplňte kontaktní osobu")]
        public string ContactPerson { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vyplňte typ verze")]
        public int VersionType { get; set; }
        public bool IsDemo { get; set; }
        public bool IsPayed { get; set; }
        public bool IsBlocked { get; set; }
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime DateOfLaunch { get; set; } = DateTime.Now;
        public DateTime DateOfPayment { get; set; } = SqlDateTime.MinValue.Value;
        [Required(ErrorMessage = "Vyplňte město")]
        public string City { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vyplňte ulici")]
        public string Street { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vyplňte PSČ")]
        public string PostNumber { get; set; } = string.Empty;
        [Required]
        [EmailAddress(ErrorMessage = "Vyplňte validní email")]
        public string Email { get; set; } = string.Empty;
        [Required]
        [Phone(ErrorMessage = "Vyplňte validní telefonní číslo")]
        public string PhoneNumber { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidTo { get; set; } = DateTime.Now.AddYears(1);
        // IListItem
        [Editable(false)]
        public string Name => NameOfOrganization;
        [Editable(false)]
        public string Description => ICO.ToString();
        [Editable(false)]
        public bool Active => ValidTo >= DateTime.Now;
    }
}
