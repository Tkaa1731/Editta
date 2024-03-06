using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural.DTO
{
    [Serializable]
    [Table("S_Opravneni")]
    public class Permition : ACreation, IListItem
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Vyplňte uživatele")]
        public int? UserId { get; set; }
        public int ModuleId { get; set; }
        [MinLength(50, ErrorMessage = "Vyplňte všechna oprávnění")]
        public string PermitionString { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidFrom { get; set; } = DateTime.Now;
        [Required(ErrorMessage = "Vyplňte datum")]
        public DateTime ValidTo { get; set; } = DateTime.Now.AddYears(1);
        // Aplication properties
        [Editable(false)]
        public string UserName { get; set; } = string.Empty;
        [Editable(false)]
        public string ModuleName { get; set; } = string.Empty;
        //IListItem
        [Editable(false)]
        public string Name => UserName;
        [Editable(false)]
        public string Description => $"{ModuleName}-přístup";
        [Editable(false)]
        public bool Active => ValidTo >= DateTime.Now;
    }
}