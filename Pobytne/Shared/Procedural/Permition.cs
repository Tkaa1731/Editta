using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Pobytne.Shared.Struct;

namespace Pobytne.Shared.Procedural
{
    [Serializable]
    [Table("S_Opravneni")]
    public class Permition : ICreation
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ModuleId { get; set; }
        public string PermitionString { get; set; } = string.Empty;
        [Editable(false)]
        public string ModuleName { get; set; } = string.Empty;
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }
        public int CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        [Editable(false)]
        public string CreationUserName { get; set; } = string.Empty;

        public bool CheckPemition(string quaery)
        {
            return true;
        }
    }
}
