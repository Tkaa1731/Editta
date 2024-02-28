using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
    [Serializable]
    [Table("S_Opravneni")]
    public class Permition : ICreation, IListItem
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; } 
        public int ModuleId { get; set; }
        public string PermitionString { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        // Aplication properties
        [Editable(false)]
        public string ModuleName { get; set; } = string.Empty;
        [Editable(false)]
        public string CreationUserName { get; set; } = string.Empty;
        [Editable(false)]
        public string Name => $"User {UserId} in {ModuleName}";
        [Editable(false)]
        public string Description => PermitionString;
        [Editable(false)]
        public bool Active => ValidTo >= DateTime.Now;
    }
}