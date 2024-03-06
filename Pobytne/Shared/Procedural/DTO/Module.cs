using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pobytne.Shared.Struct;


namespace Pobytne.Shared.Procedural.DTO
{
    [Serializable]
    [Table("S_Moduly")]
    public class Module : ACreation, IListItem
    {
        [Key]
        [Editable(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vyplňte název modulu")]
        public string ModuleName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vyplňte zkrácený název")]
        public string ModuleShortName { get; set; } = string.Empty;
        public int LicenseNumber { get; set; }
        [Required(ErrorMessage = "Vyplňte typ evidence")]
        [Range(0, 4, ErrorMessage = "Vyplňte číslo v rozsahu 0-4")]
        public int EvidenceType { get; set; }
        public bool OnlyUsersByIdOfModule { get; set; }
        //IListItem
        [Editable(false)]
        public string Name => ModuleName;
        [Editable(false)]
        public string Description => $"Typ evidence: {EvidenceType}";
        [Editable(false)]
        public bool Active => true;
    }
}
