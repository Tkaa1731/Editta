using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pobytne.Shared.Struct;


namespace Pobytne.Shared.Procedural
{
    [Serializable]
    [Table("S_Moduly")]
    public class Module : IListItem
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter ModuleName")]
        public string ModuleName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Enter ShortName of Module")]
        public string ModuleShortName { get; set; } = string.Empty;
        public int LicenseNumber { get; set; }
        [Required(ErrorMessage = "Enter Type of evidence")]
        [Range(0,4,ErrorMessage = "Enter Type of evidence in the rage of  0-4")]
        public int EvidenceType { get; set; }
        public bool OnlyUsersByIdOfModule { get; set; }
        public int CreationUserId { get; set; }
        public DateTime CreationDate{ get; set; }
        [Editable(false)]
        public string CreationUserName { get; set; } = string.Empty;
        [Editable(false)]
        public string Name => ModuleName;
        [Editable(false)]
        public string Description => $"Type of evidence {EvidenceType}";
    }
}
