using Dapper;
using Pobytne.Shared.Struct;


namespace Pobytne.Shared.Procedural
{
    [Serializable]
    [Table("S_Moduly")]
    public class Module : IListItem
    {
        [Key]
        public int Id { get; set; }
        public string ModuleName { get; set; } = string.Empty;
        public string ModuleShortName { get; set; } = string.Empty;
        public int LicenseNumber { get; set; }
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
        //[Editable(false)]
        //public Type Type { get => typeof(Module); }
    }
}
