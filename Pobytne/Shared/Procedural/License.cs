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
        public int LicenseNumber { get; set; }
        public int ICO { get; set; }
        public string NameOfOrganization { get; set; } = string.Empty;
        public string ContactPerson { get; set; } = string.Empty;
        public int VersionType { get; set; }
        public bool IsDemo { get; set; }
        public bool IsPayed { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime DateOfLaunch { get; set; }
        public DateTime DateOfPayment { get; set; }
        public string City { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string PostNumber { get; set; } = string.Empty;
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Phone]
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        [Editable(false)]
        public string CreationUserName { get; set; } = string.Empty;
        [Editable(false)]
        public string Name => ICO.ToString();
        [Editable(false)]
        public string Description => NameOfOrganization;
        //[Editable(false)]
        //public Type Type { get => typeof(License); }
    }
}
